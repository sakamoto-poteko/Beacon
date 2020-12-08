using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Beacon.Client.Settings;
using Beacon.Common;
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
            services.AddHttpClient<BeaconHttpClient>(client =>
            {
                client.BaseAddress = new Uri(serverConfiguration.Endpoint);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("BeaconClient", "1.0.0"));
            }).AddHttpMessageHandler<AuthorizedHttpHandler>();
        }

        protected virtual void ConfigureOneTimeConfigureService(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<OneTimeConfigureService>();
        }

        public virtual void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddMsalClient("4c975b25-5425-4c0c-a209-2690835c1260", AuthorizationConstants.MsaTenant);

            ConfigureQuartz(context, services);

            ConfigureAuthorizedHttpClient(context, services);

            services.AddOptions<ServerConfiguration>("ServerConfiguration");
            // TODO: add validator

            services.AddSingleton<IIPRetrievingService, IpRetrievingService>();
            services.AddSingleton<IIPUpdateService, IPUpdateService>();
            services.AddSingleton<IIPUpdateScheduler, IPUpdateScheduler>();

            services.AddTransient<IPUpdateJob>();

            ConfigureOneTimeConfigureService(context, services);

            services.AddLogging();
        }

        public virtual void ConfigureHostBuilder(IHostBuilder hostBuilder)
        {

        }
    }
}
