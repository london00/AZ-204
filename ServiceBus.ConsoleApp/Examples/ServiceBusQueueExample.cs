using Azure.Messaging.ServiceBus;

namespace MessageBasedCommunication.ConsoleApp.ConsoleApp
{
    public class ServiceBusQueueExample : IServiceBusQueueExample
    {
        private readonly IServiceBusQueueHelper serviceBusQueueHelper;

        public ServiceBusQueueExample(IServiceBusQueueHelper serviceBusQueueHelper)
        {
            this.serviceBusQueueHelper = serviceBusQueueHelper;
        }

        public async Task Execute()
        {
            await serviceBusQueueHelper.SendAsync(new { message = "Hola soy un queue!" });

            await serviceBusQueueHelper.ProccessAsync(ProcessSuccess, ProccessError);
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