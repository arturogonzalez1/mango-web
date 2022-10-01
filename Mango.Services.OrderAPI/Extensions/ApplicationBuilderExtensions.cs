using Mango.Services.OrderAPI.Messaging;

namespace Mango.Services.OrderAPI.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IAzureServiceBusConsumer ServiceBusConsumer { get; set; }

        public static IApplicationBuilder UseAzureServiceBusConsumer(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var serviceBusConsumer = scope.ServiceProvider.GetService<IAzureServiceBusConsumer>();
            // var serviceBusConsumer = app.ApplicationServices.GetService<IAzureServiceBusConsumer>();
            var hostApplicationLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            if (serviceBusConsumer != null)
            {
                ServiceBusConsumer = serviceBusConsumer;
            }
            else
            {
                throw new Exception("Error on getting IAzureServiceBusConsumer service");
            }

            if (hostApplicationLifetime != null)
            {
                hostApplicationLifetime.ApplicationStarted.Register(OnStart);
                hostApplicationLifetime.ApplicationStopped.Register(OnStop);
            }
            else
            {
                throw new Exception("Error on getting IHostApplicationLifetime service");
            }

            return app;
        }

        private static void OnStart()
        {
            ServiceBusConsumer.Start();
        }

        private static void OnStop()
        {
            ServiceBusConsumer.Stop();
        }
    }
}
