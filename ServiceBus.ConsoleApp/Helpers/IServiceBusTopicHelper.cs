using Azure.Messaging.ServiceBus;

namespace ServiceBus.ConsoleApp
{
    public interface IServiceBusTopicHelper
    {
        Task ProccessAsync(Func<ProcessMessageEventArgs, Task> MessageHandler, Func<ProcessErrorEventArgs, Task> ErrorHandler);
        Task SendAsync(object body);
    }
}