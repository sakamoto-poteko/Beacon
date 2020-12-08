using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Beacon.Common;
using Microsoft.Extensions.Logging;

namespace Beacon.Client
{
    public class IpRetrievingService : IIPRetrievingService
    {
        private readonly ILogger<IpRetrievingService> logger;
        public event Action<IList<NetworkInterfaceIPModel>> IPAddressChanged;

        public IpRetrievingService(ILogger<IpRetrievingService> logger)
        {
            this.logger = logger;
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            logger.LogInformation("IP address has changed in this computer");
            IPAddressChanged?.Invoke(GetIpForAllInterfaces());
        }

        public IList<NetworkInterfaceIPModel> GetIpForAllInterfaces()
        {
            List<NetworkInterfaceIPModel> infoList = new List<NetworkInterfaceIPModel>();

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                List<IPAddress> ipList = new List<IPAddress>();

                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork || ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ipList.Add(ip.Address);
                        }
                    }
                }

                if (ipList.Count > 0)
                {
                    NetworkInterfaceIPModel info = new NetworkInterfaceIPModel
                    {
                        InterfaceId = networkInterface.Id,
                        InterfaceName = networkInterface.Name,
                        IPAddresses = ipList
                    };

                    infoList.Add(info);
                }
            }

            return infoList;
        }
    }
}
