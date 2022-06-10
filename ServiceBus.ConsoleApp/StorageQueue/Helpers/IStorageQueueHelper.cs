namespace MessageBasedCommunication.ConsoleApp.StorageQueue.Helpers
{
    public interface IStorageQueueHelper
    {
        Task MessageProccessedAsync<T>(T messageBody) where T : QueueMessageBase;
        Task<T> ReceiveMessageAsync<T>() where T: QueueMessageBase;
        Task SendMessageAsync<T>(T body) where T : QueueMessageBase;
    }
}