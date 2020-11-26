using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beacon
{
    public class IpUploadingService : IIpUploadingService
    {
        private readonly IIpRetrivingService ipRetrivingService;
        private string computerName;

        public IpUploadingService(IIpRetrivingService ipRetrivingService)
        {
            this.ipRetrivingService = ipRetrivingService;
            this.ipRetrivingService.IpAddressChanged += IpRetrivingService_IpAddressChanged;

            this.computerName = Environment.MachineName;
        }

        public Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo)
        {
            throw new NotImplementedException();
        }

        private void IpRetrivingService_IpAddressChanged(IList<NicIpInfo> ipInfoList)
        {
            SendIpAsync(this.computerName, ipInfoList);
        }
    }
}
