using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MessageBasedCommunication.ConsoleApp.StorageQueue.Helpers
{
    public class StorageQueueHelper: IStorageQueueHelper
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

        public async Task<T> ReceiveMessageAsync<T>()
        {
            var queueClient = await this.Init<T>();

            var message = await queueClient.ReceiveMessageAsync();

            if (message.GetRawResponse().IsError)
            {
                throw new NullReferenceException("Message couldn´t be proccessed");
            }

            logger.LogInformation("Message received");
            logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(message.Value));

            var parsedMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(message.Value.Body.ToString());

            return parsedMessage;
        }

        public async Task SendMessageAsync<T>(T body)
        {
            var queueClient = await this.Init<T>();

            var result = await queueClient.SendMessageAsync(Newtonsoft.Json.JsonConvert.SerializeObject(body));

            logger.LogInformation("Message sent");
            logger.LogInformation(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
    }
}