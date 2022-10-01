using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus
{
    public class AzureServiceBusMessageBus : IMessageBus
    {
        private string connectionString = "Endpoint=sb://mango-restaurant.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=50agRpBwTQ2H8FsXDhOgE+IycU1aG1C0tVf2ZM5aQ2M=";
        public async Task PublishMessage(BaseMessage message, string topicName)
        {
            var client = new ServiceBusClient(connectionString);
            var senderClient = client.CreateSender(topicName);

            var jsonMessage = JsonConvert.SerializeObject(message);

            var finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await senderClient.SendMessageAsync(finalMessage);
            await senderClient.DisposeAsync();
        }
    }
}
