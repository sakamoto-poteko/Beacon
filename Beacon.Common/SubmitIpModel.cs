using System;
using System.Collections.Generic;

namespace Beacon.Common
{
    public class NicIpModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IList<string> Addresses { get; set; }
    }

    public class SubmitIpModel
    {
        public string ComputerName { get; set; }

        public IList<NicIpModel> NicIp { get; set; }
    }
}
