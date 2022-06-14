using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunctionApp.HttpTrigger.SginalR
{
    public class SignalRExampleFunction
    {
        private const string HUB_NAME = "notifications";
        private readonly ILogger<SignalRExampleFunction> logger;

        public SignalRExampleFunction(ILogger<SignalRExampleFunction> logger)
        {
            this.logger = logger;
        }

        [FunctionName("negotiate")]
        public SignalRConnectionInfo GetOrderNotificationsSignalRInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get","post", "options")] HttpRequest req,
        [SignalRConnectionInfo(HubName = HUB_NAME)] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }

        [FunctionName("PlacedOrderNotification")]
        public async Task Run(
        [QueueTrigger("orders")] Order order,
        [SignalR(HubName = HUB_NAME)] IAsyncCollector<SignalRMessage> signalRMessages,
        ILogger log)
        {
            log.LogInformation($"Sending notification for {order}");

            await signalRMessages.AddAsync(
                new SignalRMessage
                {
                    Target = "productOrdered",
                    Arguments = new[] { order }
                });
        }

        public class Order
        {
            public string CustomerName { get; set; }
            public string Product { get; set; }
        }
    }
}
