using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beacon.Client
{
    public interface IIpUploadingService
    {
        public Task SendIpAsync();

        public Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo);
    }
}
