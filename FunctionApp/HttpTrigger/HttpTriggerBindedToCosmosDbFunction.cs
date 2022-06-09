using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionApp.HttpTrigger
{
    public static class HttpTriggerBindedToCosmosDbFunction
    {
        [FunctionName("HttpTriggerBindedToCosmosDbFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "people/{id:int}")] HttpRequest req,
            [CosmosDB("School", "People", ConnectionStringSetting = "AzureCosmosDb")] IEnumerable<PersonAggregate> peopleClient,
            [CosmosDB("School", "Logs", ConnectionStringSetting = "AzureCosmosDb")] out LogAggregate logAggregate,
            [Bind("id")] int id,
            ILogger log)
        
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            PersonAggregate person = GetPerson(peopleClient, id);

            string responseMessage = person is null ? "Person was not find" : $"Person with name {person.Name} was found. Id: {person.Id}";

            logAggregate = new LogAggregate { Id = Guid.NewGuid().ToString(), Message = responseMessage };

            return new OkObjectResult(responseMessage);
        }

        private static PersonAggregate GetPerson(IEnumerable<PersonAggregate> client, int id)
        {
            var person = client.FirstOrDefault(x => x.Id == id.ToString());

            return person;
        }
    }

    public class PersonAggregate
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class LogAggregate 
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}
