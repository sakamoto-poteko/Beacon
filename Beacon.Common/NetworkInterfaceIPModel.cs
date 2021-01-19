using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Beacon.Common
{
    public class NetworkInterfaceIPModel
    {
        public string InterfaceId { get; set; }
        
        public string InterfaceName { get; set; }
        
        public IEnumerable<IPAddress> IPAddresses { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }
}
