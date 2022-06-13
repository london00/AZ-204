using FunctionApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FunctionApp.HttpTrigger
{
    public static class HttpTriggerPoolingFunction
    {
        [FunctionName("HttpTriggeGetPeopleFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "people")] HttpRequest req,
            [CosmosDB("School", "People", ConnectionStringSetting = "AzureCosmosDb")] IEnumerable<PersonAggregate> peopleClient,
            ILogger log)

        {
            return new OkObjectResult(peopleClient);
        }
    }
}
