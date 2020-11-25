using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Beacon
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((ctx, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((ctx, services) =>
                {
                    ConfigureServices(services);
                })
                .ConfigureLogging((ctx, loggingBuilder) =>
                {
                    loggingBuilder.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                });

            await builder.RunConsoleAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IIpRetrivingService, IpRetrivingService>();
        }
    }
}
