using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace MessageBasedCommunication.ConsoleApp.ServiceBus.Helpers
{
    public class ServiceBusQueueHelper : IServiceBusQueueHelper
    {
        private readonly IConfiguration configuration;

        private const string QUEUE_NAME = "salesqueue";

        private readonly ServiceBusSender sender;
        private readonly ServiceBusProcessor processor;

        public ServiceBusQueueHelper(IConfiguration configuration)
        {
            this.configuration = configuration;

            // Create a ServiceBusClient object using the connection string to the namespace.
            var client = new ServiceBusClient(this.configuration.GetConnectionString("ServiceBus"));

            // Create a ServiceBusSender object by invoking the CreateSender method on the ServiceBusClient object, and specifying the queue name. 
            sender = client.CreateSender(QUEUE_NAME);

            // Create a ServiceBusProcessor for the queue.
            processor = client.CreateProcessor(QUEUE_NAME, new ServiceBusProcessorOptions() { AutoCompleteMessages = true });
        }

        public async Task SendAsync(object body)
        {
            var message = Newtonsoft.Json.JsonConvert.SerializeObject(body);

            await sender.SendMessageAsync(new ServiceBusMessage(message));
        }

        public async Task ProccessAsync(Func<ProcessMessageEventArgs, Task> MessageHandler, Func<ProcessErrorEventArgs, Task> ErrorHandler)
        {
            // Specify handler methods for messages and errors.
            processor.ProcessMessageAsync += MessageHandler;
            processor.ProcessErrorAsync += ErrorHandler;

            await processor.StartProcessingAsync();
        }
    }
}
