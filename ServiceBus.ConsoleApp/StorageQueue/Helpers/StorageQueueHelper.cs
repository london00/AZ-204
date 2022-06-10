using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageBasedCommunication.ConsoleApp.StorageQueue.Helpers
{
    public class StorageQueueHelper : IStorageQueueHelper
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<StorageQueueHelper> logger;

        public StorageQueueHelper(IConfiguration configuration, ILogger<StorageQueueHelper> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        private async Task<QueueClient> Init<T>()
        {
            var queueClient = new QueueClient(configuration.GetConnectionString("AzureStorageConnectionString"), typeof(T).Name.ToLower());

            await queueClient.CreateIfNotExistsAsync();

            return queueClient;
        }

        public async Task<T> ReceiveMessageAsync<T>() where T : QueueMessageBase
        {
            var queueClient = await this.Init<T>();

            var message = await queueClient.ReceiveMessageAsync();

            if (message.GetRawResponse().IsError)
            {
                throw new NullReferenceException("Message couldn´t be proccessed");
            }

            logger.LogInformation("Message received");
            logger.LogInformation(JsonConvert.SerializeObject(message.Value));

            if (message.Value is null)
            {
                return null;
            }
            else
            {
                var messageBody = message.Value.Body.ToObjectFromJson<T>();

                messageBody.MessageReference = new QueueMessageBase.MessageReferenceDto
                {
                    MessageId = message.Value.MessageId,
                    PopReceived = message.Value.PopReceipt
                };
                return messageBody;
            }
        }

        public async Task MessageProccessedAsync<T>(T messageBody) where T : QueueMessageBase
        {
            var queueClient = await this.Init<T>();

            var response = await queueClient.DeleteMessageAsync(messageBody.MessageReference.MessageId, messageBody.MessageReference.PopReceived);

            if (response.IsError)
            {
                throw new Exception("Message be removed");
            }

            logger.LogInformation("Message removed");
        }

        public async Task SendMessageAsync<T>(T messageBody) where T : QueueMessageBase
        {
            var queueClient = await this.Init<T>();

            var result = await queueClient.SendMessageAsync(JsonConvert.SerializeObject(messageBody));

            messageBody.MessageReference = new QueueMessageBase.MessageReferenceDto
            {
                MessageId = result.Value.MessageId,
                PopReceived = result.Value.PopReceipt
            };

            logger.LogInformation("Message sent");
            logger.LogInformation(JsonConvert.SerializeObject(result));
        }
    }
}