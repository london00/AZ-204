using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp.CosmosTrigger
{
    public static class CosmosFunction
    {
        [FunctionName("CosmosFunction")]
        public static void Run([CosmosDBTrigger(
            databaseName: "School",
            collectionName: "People",
            ConnectionStringSetting = "AzureCosmosDb",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input,
            ILogger log)
        {
            log.LogInformation("Documents modified " + input.Count);

            if (input != null && input.Count > 0)
            {

                log.LogInformation("Person Id " + input[0].Id);
                log.LogInformation("Person Name " + input[0].GetPropertyValue<String>("Name"));
            }
        }
    }
}
