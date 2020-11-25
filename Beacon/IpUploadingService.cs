using System;
using System.Collections.Generic;
using System.Text;

namespace Beacon
{
    public class IpUploadingService
    {
        private readonly IIpRetrivingService ipRetrivingService;

        public IpUploadingService(IIpRetrivingService ipRetrivingService)
        {
            this.ipRetrivingService = ipRetrivingService;
            this.ipRetrivingService.IpAddressChanged += IpRetrivingService_IpAddressChanged;
        }

        private void IpRetrivingService_IpAddressChanged(IList<NicIpInfo> ipInfoList)
        {
            throw new NotImplementedException();
        }
    }
}
