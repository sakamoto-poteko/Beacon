using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeaconClient.Exceptions;
using Microsoft.Identity.Client;

namespace BeaconClient
{
    public class AuthorizationTokenManager : IAuthorizationTokenManager
    {
        private readonly IPublicClientApplication publicClientApplication;
        private readonly List<string> _scopes = new List<string>();

        public AuthorizationTokenManager(IPublicClientApplication publicClientApplication)
        {
            this.publicClientApplication = publicClientApplication;
        }

        public bool AllowInteractiveLogin { get; set; }

        public IEnumerable<string> Scopes => _scopes;

        public void AddScope(string scope)
        {
            _scopes.Add(scope);
        }

        public void RemoveScope(string scope)
        {
            _scopes.Remove(scope);
        }

        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult result;
            try
            {
                var accounts = await publicClientApplication.GetAccountsAsync();

                try
                {
                    result = await publicClientApplication.AcquireTokenSilent(new[] { "" }, accounts.FirstOrDefault()).ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    if (!AllowInteractiveLogin)
                    {
                        throw new InteractiveLoginRequiredException("An interactive login is required for this authorization");
                    }

                    result = await publicClientApplication.AcquireTokenInteractive(new[] { "" }).ExecuteAsync();
                }
            }
            catch (MsalException exception)
            {
                throw new AuthorizationTokenException(exception.Message, exception);
            }

            return result.IdToken;
        }
    }
}
