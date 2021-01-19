using System;
using System.Net.Http;
using System.Threading.Tasks;
using Beacon.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beacon.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<BeaconServerAuthorizationMessageHandler>();
            builder.Services
                .AddHttpClient<BeaconHttpClient>(client =>
                {
                    client.BaseAddress = new Uri("https://beacon.azurewebsites.net");
                }).AddHttpMessageHandler<BeaconServerAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddMsalAuthentication(options =>
            {
                builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
                options.ProviderOptions.DefaultAccessTokenScopes = new[] { "User.Read" };
            });

            await builder.Build().RunAsync();
        }
    }
}
