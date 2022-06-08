using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace FunctionApp.HttpTrigger
{
    public static class GithubWebhookFunction
    {
        [FunctionName("GithubWebhookFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a GitHub request.");
            
            await GitHubWebHubHelper.CompareSignatureAsync(req);

            log.LogInformation("x-github-event: " + req.Headers["x-github-event"].ToString());

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Print Github body
            log.LogInformation(requestBody);

            return new OkObjectResult("GItHub WebHook call received");
        }
    }

    public static class GitHubWebHubHelper
    {
        private const string MY_PRIVATE_KEY = "mykey";

        public static async Task CompareSignatureAsync(HttpRequest req)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(MY_PRIVATE_KEY);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            var gitHubSignature = req.Headers["x-hub-signature"].ToString();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            var shaSignature = hmacsha1.ComputeHash(encoding.GetBytes(requestBody));

            if (string.Equals(gitHubSignature, shaSignature.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                throw new CryptographicUnexpectedOperationException("GitHub WebHook ket is not valid.");
            }
        }
    }
}
