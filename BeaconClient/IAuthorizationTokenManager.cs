using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beacon.Client
{
    public interface IAuthorizationTokenManager
    {
        public bool AllowInteractiveLogin { get; set; }

        public IEnumerable<string> Scopes { get; }

        public void AddScope(string scope);

        public void RemoveScope(string scope);
        
        public Task<string> GetTokenAsync();
    }
}
