using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace BeaconClient
{
    public class AuthorizedHttpHandler : HttpClientHandler
    {
        private readonly IAuthorizationTokenManager authorizationTokenManager;

        public AuthorizedHttpHandler(IAuthorizationTokenManager authorizationTokenManager)
        {
            this.authorizationTokenManager = authorizationTokenManager;
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string bearerToken = await authorizationTokenManager.GetTokenAsync();

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
