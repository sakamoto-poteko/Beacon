using System;
using System.Collections;
using System.Collections.Generic;

namespace Beacon
{
    public interface IIpRetrivingService
    {
        public event Action<IList<NicIpInfo>> IpAddressChanged;

        public IList<NicIpInfo> GetIpForAllNics();
    }
}