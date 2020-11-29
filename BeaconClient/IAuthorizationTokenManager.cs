using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconClient
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
