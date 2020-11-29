using System;
using System.Net.Http;
using BeaconClient.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace BeaconClient
{
    class Program
    {
        private const string MSATenant = "9188040d-6c67-4c5b-b112-36a304b66dad";

        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddMsalClient("4c975b25-5425-4c0c-a209-2690835c1260", MSATenant);

            services.AddQuartz(config =>
            {
                config.SchedulerName = IpUploadSchedulerConstants.SchedulerId;
                config.SchedulerId = IpUploadSchedulerConstants.SchedulerId;

                config.UseMicrosoftDependencyInjectionScopedJobFactory();

                config.UseSimpleTypeLoader();
                config.UseInMemoryStore();
                config.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 2;
                });
            });
            services.AddQuartzHostedService();

            var serverConfiguration = new ServerConfiguration();
            context.Configuration.GetSection("ServerConfiguration").Bind(serverConfiguration);

            services.AddSingleton<HttpClient>(sp =>
            {
                var httpMessageHandler = sp.GetService<AuthorizedHttpHandler>();
                var httpClient = new HttpClient(httpMessageHandler)
                {
                    BaseAddress = new Uri(serverConfiguration.Endpoint)
                };

                httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("BeaconClient", "1.0.0"));

                return httpClient;
            });

            services.AddOptions<ServerConfiguration>("ServerConfiguration");
            // TODO: add validator

            services.AddSingleton<AuthorizedHttpHandler>();
            services.AddSingleton<IAuthorizationTokenManager, AuthorizationTokenManager>();
            services.AddSingleton<IIpRetrivingService, IpRetrivingService>();
            services.AddSingleton<IIpUploadingService, IpUploadingService>();
            services.AddSingleton<IIpUploadingScheduler, IpUploadingScheduler>();

            services.AddTransient<IpUploadingJob>();

            services.AddHostedService<OneTimeConfigureService>();

            services.AddLogging();
        }
    }
}
