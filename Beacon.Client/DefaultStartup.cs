using System;
using System.Net.Http;
using Beacon.Client.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;

namespace Beacon.Client
{
    public class DefaultStartup
    {
        protected virtual void ConfigureQuartz(HostBuilderContext context, IServiceCollection services)
        {
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
        }

        protected virtual void ConfigureAuthorizedHttpClient(HostBuilderContext context, IServiceCollection services)
        {
            var serverConfiguration = new ServerConfiguration();
            context.Configuration.GetSection("ServerConfiguration").Bind(serverConfiguration);
            services.AddSingleton<AuthorizedHttpHandler>();
            services.AddSingleton<IAuthorizationTokenManager, AuthorizationTokenManager>();
            services.AddSingleton(sp =>
            {
                var httpMessageHandler = sp.GetService<AuthorizedHttpHandler>();
                var httpClient = new HttpClient(httpMessageHandler)
                {
                    BaseAddress = new Uri(serverConfiguration.Endpoint)
                };

                // TODO: version info
                httpClient.DefaultRequestHeaders.UserAgent.Add(
                    new System.Net.Http.Headers.ProductInfoHeaderValue("BeaconClient", "1.0.0"));

                return httpClient;
            });
        }

        protected virtual void ConfigureOneTimeConfigureService(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<OneTimeConfigureService>();
        }

        protected void ConfigureCoreServices(HostBuilderContext context, IServiceCollection services, bool useWam)
        {
            services.AddMsalClient("4c975b25-5425-4c0c-a209-2690835c1260", AuthorizationConstants.MsaTenant, useWam);

            ConfigureQuartz(context, services);

            ConfigureAuthorizedHttpClient(context, services);

            services.AddOptions<ServerConfiguration>("ServerConfiguration");
            // TODO: add validator

            services.AddSingleton<IIpRetrievingService, IpRetrievingService>();
            services.AddSingleton<IIpUploadingService, IpUploadingService>();
            services.AddSingleton<IIpUploadingScheduler, IpUploadingScheduler>();

            services.AddTransient<IpUploadingJob>();

            ConfigureOneTimeConfigureService(context, services);

            services.AddLogging();
        }

        public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            ConfigureCoreServices(context, services, false);
        }

        public virtual void ConfigureHostBuilder(IHostBuilder hostBuilder)
        {

        }
     }
}
