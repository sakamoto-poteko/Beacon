﻿using System.Collections.Generic;
using System.Net;

namespace Beacon
{
    public record NicIpInfo(string Id, string NicName, IEnumerable<IPAddress> Address);
}
