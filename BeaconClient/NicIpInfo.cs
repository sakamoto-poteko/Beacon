using System.Collections.Generic;
using System.Net;

namespace Beacon.Client
{
    public class NicIpInfo
    {
        public string Id { get; set; }

        public string NicName { get; set; }

        public IEnumerable<IPAddress> Address { get; set; }
    }
}
