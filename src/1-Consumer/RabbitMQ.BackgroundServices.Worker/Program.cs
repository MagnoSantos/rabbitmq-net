using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Infra.CrossCutting.IoC;
using System.IO;
using System.Threading.Tasks;

namespace RabbitMQ.Consumer.Worker
{
    internal static class Program
    {
        private static async Task Main(string[] args)
            => await CreateHostBuilder(args).Build().RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.ConfigureContainer(hostContext.Configuration);
                    services.AddHostedService<BackgroundServiceConsumer>();
                });
    }
}