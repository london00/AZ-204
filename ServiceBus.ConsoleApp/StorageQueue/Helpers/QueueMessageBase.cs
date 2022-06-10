namespace MessageBasedCommunication.ConsoleApp.StorageQueue.Helpers
{
    public abstract class QueueMessageBase
    {
        public virtual MessageReferenceDto MessageReference { get; set; }

        public class MessageReferenceDto
        {
            public string MessageId { get; set; }
            public string PopReceived { get; set; }
        }
    }
}
