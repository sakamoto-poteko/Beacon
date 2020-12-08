using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Beacon.Client
{
    public class AuthorizedHttpHandler : DelegatingHandler
    {
        private readonly IAuthorizationTokenManager authorizationTokenManager;

        public AuthorizedHttpHandler(IAuthorizationTokenManager authorizationTokenManager)
        {
            this.authorizationTokenManager = authorizationTokenManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string bearerToken = await authorizationTokenManager.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
