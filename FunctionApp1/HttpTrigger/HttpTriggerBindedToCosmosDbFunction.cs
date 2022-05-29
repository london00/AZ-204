using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;

namespace FunctionApp.HttpTrigger
{
    public static class HttpTriggerBindedToCosmosDbFunction
    {
        [FunctionName("HttpTriggerBindedToCosmosDbFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "people/{id:int}")] HttpRequest req,
            [CosmosDB("School", "People", ConnectionStringSetting = "AzureCosmosDb")] IEnumerable<People> client,
            [Bind("id")] int id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            People person = GetPerson(client, id);

            string responseMessage = person is null ? "Person was not find" : $"Person with name {person.Name} was found. Id: {person.Id}";

            return new OkObjectResult(responseMessage);
        }

        private static People GetPerson(IEnumerable<People> client, int id)
        {
            var person = client.FirstOrDefault(x => x.Id == id.ToString());

            return person;
        }
    }

    public class People
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
