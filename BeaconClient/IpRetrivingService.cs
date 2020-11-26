using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

namespace Beacon
{
    public class IpRetrivingService : IIpRetrivingService
    {
        public event Action<IList<NicIpInfo>> IpAddressChanged;

        public IpRetrivingService()
        {
            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
        }

        private void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            IpAddressChanged?.Invoke(GetIpForAllNics());
        }

        public IList<NicIpInfo> GetIpForAllNics()
        {
            List<NicIpInfo> infoList = new List<NicIpInfo>();

            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                List<IPAddress> ipList = new List<IPAddress>();

                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipList.Add(ip.Address);
                        }
                    }
                }

                if (ipList.Count > 0)
                {
                    NicIpInfo info = new NicIpInfo(networkInterface.Id, networkInterface.Name, new List<IPAddress>());
                    infoList.Add(info);
                }
            }

            return infoList;
        }
    }
}
