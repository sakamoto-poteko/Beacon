using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace Beacon.Client
{
    public static class ConfigureExtensions
    {
        public static void AddMsalClient(this IServiceCollection services, string clientId, string tenantId, bool useWam = false)
        {
            var msalClientBuilder = PublicClientApplicationBuilder.Create(clientId);
            msalClientBuilder = useWam
                ? msalClientBuilder.WithBroker(true)
                : msalClientBuilder.WithRedirectUri("http://localhost")
                    .WithAuthority(AzureCloudInstance.AzurePublic, tenantId);
            var msalClient = msalClientBuilder.Build();

            TokenCacheHelper.EnableSerialization(msalClient.UserTokenCache);
            services.AddSingleton(msalClient);
        }
    }
}
