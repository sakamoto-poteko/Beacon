using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beacon.Client.Exceptions;
using Microsoft.Identity.Client;

namespace Beacon.Client
{
    public class AuthorizationTokenManager : IAuthorizationTokenManager
    {
        private readonly IPublicClientApplication publicClientApplication;
        private readonly List<string> scopes = new List<string>();

        public AuthorizationTokenManager(IPublicClientApplication publicClientApplication)
        {
            this.publicClientApplication = publicClientApplication;
        }

        public bool AllowInteractiveLogin { get; set; }

        public IEnumerable<string> Scopes => scopes;

        public void AddScope(string scope)
        {
            scopes.Add(scope);
        }

        public void RemoveScope(string scope)
        {
            scopes.Remove(scope);
        }

        public async Task<string> GetTokenAsync()
        {
            AuthenticationResult result;
            try
            {
                var accounts = await publicClientApplication.GetAccountsAsync();
                var msaAccount = accounts.FirstOrDefault(account => account.HomeAccountId.TenantId == AuthorizationConstants.MsaTenant);

                try
                {
                    result = await publicClientApplication.AcquireTokenSilent(Scopes, msaAccount).ExecuteAsync();
                }
                catch (MsalUiRequiredException)
                {
                    if (!AllowInteractiveLogin)
                    {
                        throw new InteractiveLoginRequiredException("An interactive login is required for this authorization");
                    }

                    result = await publicClientApplication.AcquireTokenInteractive(Scopes).WithAccount(msaAccount).ExecuteAsync();
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
