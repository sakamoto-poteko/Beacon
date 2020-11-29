using System.Collections.Generic;
using System.Net;

namespace BeaconClient
{
    public record NicIpInfo(string Id, string NicName, IEnumerable<IPAddress> Address);
}
