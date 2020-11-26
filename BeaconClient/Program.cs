using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace Beacon
{
    class Program
    {
        public static IPublicClientApplication PublicClientApp;

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var app = PublicClientApplicationBuilder.Create("4c975b25-5425-4c0c-a209-2690835c1260")
                    .WithRedirectUri("http://localhost")
                    .WithAuthority(AzureCloudInstance.AzurePublic, "9188040d-6c67-4c5b-b112-36a304b66dad")
                    .Build();

            var authResult = await app.AcquireTokenInteractive(new[] { "" }).ExecuteAsync();

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
