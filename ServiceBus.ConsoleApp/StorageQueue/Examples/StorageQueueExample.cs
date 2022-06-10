using MessageBasedCommunication.ConsoleApp.StorageQueue.Helpers;

namespace MessageBasedCommunication.ConsoleApp.StorageQueue.Examples
{
    public class StorageQueueExample : IStorageQueueExample
    {
        private readonly IStorageQueueHelper storageQueueHelper;

        public StorageQueueExample(IStorageQueueHelper storageQueueHelper)
        {
            this.storageQueueHelper = storageQueueHelper;
        }

        public async Task Execute()
        {
            await storageQueueHelper.SendMessageAsync(new PersonMessage
            {
                Name = "Geiser: " + Guid.NewGuid()
            });

            var person = await storageQueueHelper.ReceiveMessageAsync<PersonMessage>();
        }

        public class PersonMessage
        {
            public string Name { get; set; }
        }
    }
}
