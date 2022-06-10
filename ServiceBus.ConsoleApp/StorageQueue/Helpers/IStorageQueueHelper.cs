
using Azure.Storage.Queues;

namespace MessageBasedCommunication.ConsoleApp.StorageQueue.Helpers
{
    public interface IStorageQueueHelper
    {
        Task<T> ReceiveMessageAsync<T>();
        Task SendMessageAsync<T>(T body);
    }
}