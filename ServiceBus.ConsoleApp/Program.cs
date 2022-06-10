using MessageBasedCommunication.ConsoleApp.ServiceBus.Examples;
using MessageBasedCommunication.ConsoleApp.ServiceBus.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MessageBasedCommunication.ConsoleApp
{
    public class Program
    {
        public Program()
        {

        }

        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await EerviceBusPOC(host);

            await host.WaitForShutdownAsync();
        }

        private static async Task EerviceBusPOC(IHost host)
        {
            var serviceBusQueueExample = host.Services.GetService<IServiceBusQueueExample>();
            await serviceBusQueueExample.Execute();

            var serviceBusTopicExample = host.Services.GetService<IServiceBusTopicExample>();
            await serviceBusTopicExample.Execute();
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