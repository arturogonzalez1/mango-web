using Azure.Messaging.ServiceBus;
using Mango.MessageBus;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Mango.Services.Email.Repositories;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.Email.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string subscriptionEmail;
        private readonly string orderUpdatePaymentResultTopic;

        private readonly EmailRepository _emailRepository;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _orderUpdatePaymentStatusProcessor;

        public AzureServiceBusConsumer(EmailRepository emailRepository, IConfiguration configuration)
        {
            _emailRepository = emailRepository;
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            subscriptionEmail = _configuration.GetValue<string>("SubscriptionName");
            orderUpdatePaymentResultTopic = _configuration.GetValue<string>("OrderUpdatePaymentResultTopic");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _orderUpdatePaymentStatusProcessor = client.CreateProcessor(orderUpdatePaymentResultTopic, subscriptionEmail);
        }

        public async Task Start()
        {
            _orderUpdatePaymentStatusProcessor.ProcessMessageAsync += OnOrderPaymentUpdateReceived;
            _orderUpdatePaymentStatusProcessor.ProcessErrorAsync += ErrorHandler;
            await _orderUpdatePaymentStatusProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _orderUpdatePaymentStatusProcessor.StopProcessingAsync();
            await _orderUpdatePaymentStatusProcessor.DisposeAsync();
        }

        private async Task OnOrderPaymentUpdateReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            var paymentResultMessage = JsonConvert.DeserializeObject<UpdatePaymentResultMessage>(body);

            if (paymentResultMessage != null)
            {
                try
                {
                    await _emailRepository.SendAndLogEmail(paymentResultMessage);
                    await args.CompleteMessageAsync(args.Message);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
