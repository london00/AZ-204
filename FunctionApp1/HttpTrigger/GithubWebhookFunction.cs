using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

            await GitHubWebHubHelper.CompareSignatureAsync(req, log);

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

        public static async Task CompareSignatureAsync(HttpRequest req, ILogger log)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(MY_PRIVATE_KEY);

            HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);

            var gitHubSignature = req.Headers["x-hub-signature"].ToString();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var shaSignature = hmacsha1.ComputeHash(encoding.GetBytes(requestBody));

            if (string.Equals(gitHubSignature, "sha1=" + ToHexString(shaSignature), StringComparison.OrdinalIgnoreCase))
            {
                log.LogInformation("GitHub WebHook key: Valid");
            }
            else
            {
                log.LogInformation("GitHub WebHook key: Invalid");

                throw new CryptographicUnexpectedOperationException("GitHub WebHook key is not valid.");
            }
        }

        private static string ToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }
    }
}
