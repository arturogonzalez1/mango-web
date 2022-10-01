using Mango.Services.Email.Messages;
using Mango.Services.Email.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailRepository _emailRepository;
        private IConnection _connection;
        private IModel _channel;

        private const string ExchangeName = "DirectPaymentUpdate_Exchange";
        private const string PaymentEmailQueueName = "PaymentEmailQueueName";

        public RabbitMQPaymentConsumer(IConfiguration configuration, EmailRepository emailRepository)
        {
            _configuration = configuration;
            _emailRepository = emailRepository;
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"],
                UserName = _configuration["RabbitMQ:UserName"],
                Password = _configuration["RabbitMQ:Password"],
            };
            
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(PaymentEmailQueueName, false, false, false, null);
            _channel.QueueBind(PaymentEmailQueueName, ExchangeName, "PaymentEmail");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var updatePaymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(content);
                HandleMessage(updatePaymentResultMessage).GetAwaiter().GetResult();

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(PaymentEmailQueueName, false, consumer);

            return Task.CompletedTask;
        }

        private async Task HandleMessage(UpdatePaymentResultMessage updatePaymentResultMessage)
        {
            if (updatePaymentResultMessage != null)
            {
                try
                {
                    await _emailRepository.SendAndLogEmail(updatePaymentResultMessage);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
