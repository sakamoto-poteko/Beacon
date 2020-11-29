using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeaconClient
{
    public interface IIpUploadingService
    {
        public Task SendIpAsync();

        public Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo);
    }
}
