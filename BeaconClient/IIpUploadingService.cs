using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beacon
{
    public interface IIpUploadingService
    {
        public Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo);
    }
}
