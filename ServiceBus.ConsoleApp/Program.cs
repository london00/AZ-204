using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageBasedCommunication.ConsoleApp.ConsoleApp
{
    public class Program
    {
        public Program()
        {

        }

        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            var serviceBusQueueExample = host.Services.GetService<IServiceBusQueueExample>();
            await serviceBusQueueExample.Execute();

            var serviceBusTopicExample = host.Services.GetService<IServiceBusTopicExample>();
            await serviceBusTopicExample.Execute();

            await host.WaitForShutdownAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddUserSecrets<Program>();
                })
                .ConfigureServices((context, services) =>
                {
                    RegisterServices(services);
                });

            return hostBuilder;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IServiceBusQueueExample, ServiceBusQueueExample>();
            services.AddScoped<IServiceBusTopicExample, ServiceBusTopicExample>();

            services.AddScoped<IServiceBusQueueHelper, ServiceBusQueueHelper>();
            services.AddScoped<IServiceBusTopicHelper, ServiceBusTopicHelper>();
        }
    }
}