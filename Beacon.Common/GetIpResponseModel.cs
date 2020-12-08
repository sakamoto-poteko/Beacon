using System;
using System.Collections.Generic;
using System.Text;

namespace Beacon.Common
{
    public class GetIPResponseModel
    {
        public class NicIPModel
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public IEnumerable<string> Addresses { get; set; }

            public DateTime LastUpdated { get; set; }
        }

        public string ComputerName { get; set; }

        public IList<NicIPModel> NicIp { get; set; }
    }
}
