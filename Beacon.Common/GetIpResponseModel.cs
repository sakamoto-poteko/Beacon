using System;
using System.Collections.Generic;
using System.Text;

namespace Beacon.Common
{
    public class GetIpResponseModel
    {
        public string ComputerName { get; set; }

        public IList<NicIpModelForGet> NicIp { get; set; }
    }
}
