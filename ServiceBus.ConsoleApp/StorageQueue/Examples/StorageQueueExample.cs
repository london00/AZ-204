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

            bool pendingMessages = false;

            do
            {
                var person = await storageQueueHelper.ReceiveMessageAsync<PersonMessage>();

                if (person is not null)
                {
                    await storageQueueHelper.MessageProccessedAsync(person);

                    pendingMessages = true;
                }
                else
                {
                    pendingMessages = false;
                }
            }
            while (pendingMessages);
        }

        public class PersonMessage : QueueMessageBase
        {
            public string Name { get; set; }
        }
    }
}
