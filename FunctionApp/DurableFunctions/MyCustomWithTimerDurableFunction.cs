using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionApp.DurableFunctions
{
    public static class MyCustomWithTimerDurableFunction
    {
        private static int secondsToWait = 0;

        [FunctionName(nameof(MyCustomWithTimerDurableFunction) + "_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "MyCustomWithTimerDurableFunction/{id:int}")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            [Bind("id")] int id,
            ILogger log)
        {
            secondsToWait = id;

            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync(nameof(MyCustomWithTimerDurableFunction), null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            HttpResponseMessage httpResponseMessage = starter.CreateCheckStatusResponse(req, instanceId);

            return httpResponseMessage;
        }

        [FunctionName(nameof(MyCustomWithTimerDurableFunction))]
        public static async Task<List<string>> RunOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            // CancellationTokenSource shares the same token for the orquestration activities.
            using (var cts = new CancellationTokenSource())
            {
                var taskAllCompletedTask = Task.WhenAll(
                    context.CallActivityAsync<string>(nameof(MyCustomWithTimerDurableFunction) + "_Hello", "Tokyo"),
                    context.CallActivityAsync<string>(nameof(MyCustomWithTimerDurableFunction) + "_Hello", "Seattle"),
                    context.CallActivityAsync<string>(nameof(MyCustomWithTimerDurableFunction) + "_Hello", "London"),
                    context.CallActivityAsync<string>(nameof(MyCustomWithTimerDurableFunction) + "_Goodbye", "Everyone")
                    );

                var timeoutTask = context.CreateTimer(context.CurrentUtcDateTime.AddSeconds(secondsToWait), cts.Token);

                Task winner = await Task.WhenAny(taskAllCompletedTask, timeoutTask);

                if (winner == timeoutTask)
                {
                    // success case
                    cts.Cancel();

                    return new List<string> { $"Orquestration cancelled due to has taken more than {secondsToWait} seconds (timeout)" };
                }
                else
                {
                    // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
                    Debug.WriteLine("Result: \n" + string.Join("\r", taskAllCompletedTask.Result));

                    return taskAllCompletedTask.Result.ToList();
                }
            }
        }

        [FunctionName(nameof(MyCustomWithTimerDurableFunction) + "_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            string message = $"Saying hello to {name}.";
            log.LogInformation(message);
            Debug.WriteLine(message);

            return $"Hello {name}!";
        }

        [FunctionName(nameof(MyCustomWithTimerDurableFunction) + "_Goodbye")]
        public static string SayGoodbye([ActivityTrigger] string name, ILogger log)
        {
            string message = $"Saying goodbye to {name}.";
            log.LogInformation(message);
            Debug.WriteLine(message);

            Thread.Sleep(TimeSpan.FromSeconds(1));

            return $"Goodbye {name}!";
        }
    }
}