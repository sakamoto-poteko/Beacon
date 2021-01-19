using System;
using System.Collections.Generic;
using Beacon.Common;

namespace Beacon.Client
{
    public interface IIPRetrievingService
    {
        public event Action<IList<NetworkInterfaceIPModel>> IPAddressChanged;

        public IList<NetworkInterfaceIPModel> GetIpForAllInterfaces();
    }
}