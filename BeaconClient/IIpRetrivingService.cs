using System;
using System.Collections;
using System.Collections.Generic;

namespace BeaconClient
{
    public interface IIpRetrivingService
    {
        public event Action<IList<NicIpInfo>> IpAddressChanged;

        public IList<NicIpInfo> GetIpForAllNics();
    }
}