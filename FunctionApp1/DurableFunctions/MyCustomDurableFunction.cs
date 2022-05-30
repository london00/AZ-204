using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp.DurableFunctions
{
    public static class MyCustomDurableFunction
    {
        [FunctionName(nameof(MyCustomDurableFunction) + "_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(nameof(MyCustomDurableFunction), null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            HttpResponseMessage httpResponseMessage = starter.CreateCheckStatusResponse(req, instanceId);

            return httpResponseMessage;
        }

        /* This is a durable function response.
            {
                "id": "45a7476ddad74a069401fe7a20d263d2",
                "statusQueryGetUri": "http://localhost:7071/runtime/webhooks/durabletask/instances/45a7476ddad74a069401fe7a20d263d2?taskHub=TestHubName&connection=Storage&code=o7ZdUkcrifOlPaejvpagaz_oNljLvQ9KATqzpeRNO9c1AzFuU_RzCA==",
                "sendEventPostUri": "http://localhost:7071/runtime/webhooks/durabletask/instances/45a7476ddad74a069401fe7a20d263d2/raiseEvent/{eventName}?taskHub=TestHubName&connection=Storage&code=o7ZdUkcrifOlPaejvpagaz_oNljLvQ9KATqzpeRNO9c1AzFuU_RzCA==",
                "terminatePostUri": "http://localhost:7071/runtime/webhooks/durabletask/instances/45a7476ddad74a069401fe7a20d263d2/terminate?reason={text}&taskHub=TestHubName&connection=Storage&code=o7ZdUkcrifOlPaejvpagaz_oNljLvQ9KATqzpeRNO9c1AzFuU_RzCA==",
                "purgeHistoryDeleteUri": "http://localhost:7071/runtime/webhooks/durabletask/instances/45a7476ddad74a069401fe7a20d263d2?taskHub=TestHubName&connection=Storage&code=o7ZdUkcrifOlPaejvpagaz_oNljLvQ9KATqzpeRNO9c1AzFuU_RzCA==",
                "restartPostUri": "http://localhost:7071/runtime/webhooks/durabletask/instances/45a7476ddad74a069401fe7a20d263d2/restart?taskHub=TestHubName&connection=Storage&code=o7ZdUkcrifOlPaejvpagaz_oNljLvQ9KATqzpeRNO9c1AzFuU_RzCA=="
            }
         */

        [FunctionName(nameof(MyCustomDurableFunction))]
        public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>(nameof(MyCustomDurableFunction) + "_Hello", "Tokyo"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(MyCustomDurableFunction) + "_Hello", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>(nameof(MyCustomDurableFunction) + "_Hello", "London"));

            // Call good by
            outputs.Add(await context.CallActivityAsync<string>(nameof(MyCustomDurableFunction) + "_Goodbye", "Everyone"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            System.Diagnostics.Debug.WriteLine("Result: \n" + string.Join("\r", outputs));

            return outputs;
        }

        [FunctionName(nameof(MyCustomDurableFunction) + "_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            string message = $"Saying hello to {name}.";
            log.LogInformation(message);
            System.Diagnostics.Debug.WriteLine(message);

            return $"Hello {name}!";
        }

        [FunctionName(nameof(MyCustomDurableFunction) + "_Goodbye")]
        public static string SayGoodbye([ActivityTrigger] string name, ILogger log)
        {
            string message = $"Saying goodbye to {name}.";
            log.LogInformation(message);
            System.Diagnostics.Debug.WriteLine(message);

            return $"Goodbye {name}!";
        }
    }
}