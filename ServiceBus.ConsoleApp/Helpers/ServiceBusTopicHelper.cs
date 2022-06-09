using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace ServiceBus.ConsoleApp
{
    public class ServiceBusTopicHelper : IServiceBusTopicHelper
    {
        private readonly IConfiguration configuration;

        //private const string CONNECTION_STRING = "Endpoint=sb://geiserservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=UX093iwwjS0S9DLZKhH7A6me3NOnfLd4zxqsNc8KQrM=";
        private const string TOPIC_NAME = "salesperformancemessages";
        private const string SUBSCRIPTION_NAME = "Geiser";

        private readonly ServiceBusSender sender;
        private readonly ServiceBusProcessor processor;

        public ServiceBusTopicHelper(IConfiguration configuration)
        {
            this.configuration = configuration;

            // Create a ServiceBusClient object using the connection string to the namespace.
            var client = new ServiceBusClient(this.configuration.GetConnectionString("ServiceBus"));

            // Create a ServiceBusSender object by invoking the CreateSender method on the ServiceBusClient object, and specifying the queue name. 
            this.sender = client.CreateSender(TOPIC_NAME);

            // Create a ServiceBusProcessor for the queue.
            this.processor = client.CreateProcessor(TOPIC_NAME, SUBSCRIPTION_NAME, new ServiceBusProcessorOptions() { AutoCompleteMessages = true });
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
