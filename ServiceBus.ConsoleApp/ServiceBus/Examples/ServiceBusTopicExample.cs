using Azure.Messaging.ServiceBus;
using MessageBasedCommunication.ConsoleApp.ServiceBus.Helpers;

namespace MessageBasedCommunication.ConsoleApp.ServiceBus.Examples
{
    public class ServiceBusTopicExample : IServiceBusTopicExample
    {
        private readonly IServiceBusTopicHelper serviceBusTopicHelper;

        public ServiceBusTopicExample(IServiceBusTopicHelper serviceBusTopicHelper)
        {
            this.serviceBusTopicHelper = serviceBusTopicHelper;
        }

        public async Task Execute()
        {
            await serviceBusTopicHelper.SendAsync(new { message = "Hola soy un topic!" });

            await serviceBusTopicHelper.ProccessAsync(ProcessSuccess, ProccessError);
        }

        protected Task ProccessError(ProcessErrorEventArgs arg)
        {
            Console.WriteLine("Error");
            Console.WriteLine(arg.Exception.Message);

            return Task.CompletedTask;
        }

        protected async Task ProcessSuccess(ProcessMessageEventArgs arg)
        {
            Console.WriteLine("Success");
            Console.WriteLine(arg.Message.Body.ToString());

            await arg.CompleteMessageAsync(arg.Message);
        }
    }
}