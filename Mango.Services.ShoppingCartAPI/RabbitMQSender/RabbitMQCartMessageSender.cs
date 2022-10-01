using Mango.MessageBus;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.Services.ShoppingCartAPI.RabbitMQSender
{
    public class RabbitMQCartMessageSender : IRabbitMQCartMessageSender
    {
        private readonly string _hostname;
        private readonly string _username;
        private readonly string _password;

        private readonly IConfiguration _configuration;
        private IConnection _connection;

        public RabbitMQCartMessageSender(IConfiguration configuration)
        {
            _configuration = configuration;

            _hostname = _configuration["RabbitMQ:HostName"];
            _username = _configuration["RabbitMQ:UserName"];
            _password = _configuration["RabbitMQ:Password"];
        }
        public void SendMessage(BaseMessage message, string queueName)
        {
            CreateConnectionIfNotExists();

            using var channel = _connection.CreateModel();
            channel.QueueDeclare(queueName, false, false, false, arguments: null);
            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);
            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
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
}
