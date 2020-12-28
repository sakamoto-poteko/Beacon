using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace Beacon.Client
{
    public static class ConfigureExtensions
    {
        public static void AddMsalClient(this IServiceCollection services, string clientId, string tenantId, bool useWam = false)
        {
            var msalClientBuilder = PublicClientApplicationBuilder.Create(clientId);
            msalClientBuilder = useWam // FIXME: this won't work until 4.25 of MSAL
                ? msalClientBuilder.WithBroker(true)
                : msalClientBuilder.WithRedirectUri("http://localhost") // WAM doesn't have redirect uri
                    .WithAuthority(AzureCloudInstance.AzurePublic, tenantId);
            IPublicClientApplication msalClient = msalClientBuilder.Build();

            TokenCacheHelper.EnableSerialization(msalClient.UserTokenCache);
            services.AddSingleton<IPublicClientApplication>(msalClient);
        }
    }
}
