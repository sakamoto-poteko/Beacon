using System;
using System.Collections.Generic;

namespace Beacon.Client
{
    public interface IIpRetrievingService
    {
        public event Action<IList<NicIpInfo>> IpAddressChanged;

        public IList<NicIpInfo> GetIpForAllInterfaces();
    }
}