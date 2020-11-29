using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace Beacon.Client
{
    public static class ConfigureExtensions
    {
        public static void AddMsalClient(this IServiceCollection services, string clientId, string tenantId)
        {
            var msalClient = PublicClientApplicationBuilder.Create(clientId)
                .WithRedirectUri("http://localhost")
                .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                .Build();
            
            TokenCacheHelper.EnableSerialization(msalClient.UserTokenCache);
            services.AddSingleton(msalClient);
        }
    }
}
