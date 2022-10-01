using Mango.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.PaymentAPI.RabbitMQSender;
public class RabbitMQPaymentMessageSender : IRabbitMQPaymentMessageSender
{
    private readonly string _hostname;
    private readonly string _username;
    private readonly string _password;

    private readonly IConfiguration _configuration;
    private IConnection _connection;

    private const string ExchangeName = "DirectPaymentUpdate_Exchange";
    private const string PaymentEmailQueueName = "PaymentEmailQueueName";
    private const string PaymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

    public RabbitMQPaymentMessageSender(IConfiguration configuration)
    {
        _configuration = configuration;

        _hostname = _configuration["RabbitMQ:HostName"];
        _username = _configuration["RabbitMQ:UserName"];
        _password = _configuration["RabbitMQ:Password"];
    }
    public void SendMessage(BaseMessage message)
    {
        CreateConnectionIfNotExists();

        using var channel = _connection.CreateModel();
        channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, durable: false);

        channel.QueueDeclare(PaymentEmailQueueName, false, false, false, null);
        channel.QueueDeclare(PaymentOrderUpdateQueueName, false, false, false, null);

        channel.QueueBind(PaymentEmailQueueName, ExchangeName, "PaymentEmail");
        channel.QueueBind(PaymentOrderUpdateQueueName, ExchangeName, "PaymentOrder");

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: ExchangeName, "PaymentEmail", basicProperties: null, body: body);
        channel.BasicPublish(exchange: ExchangeName, "PaymentOrder", basicProperties: null, body: body);
    }

    private void CreateConnectionIfNotExists()
    {
        if (_connection == null)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                // Log exception
                throw;
            }
        }
    }
}
