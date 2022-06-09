using FunctionApp;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionApp
{
    // inherit FunctionsStartup
    public class Startup : FunctionsStartup
    {
        // override configure method
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();


            // register your other services
            if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
            {
                config.AddUserSecrets<Startup>(true);
            }

            var settings = config.Build();

            builder.Services.AddSingleton<IConfiguration>(settings);
        }
    }
}