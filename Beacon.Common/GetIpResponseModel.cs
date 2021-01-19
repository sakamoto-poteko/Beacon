using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Beacon.Common
{
    public class GetIPResponseModel
    {
        public class NicIPModel
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("addresses")]
            public IEnumerable<string> Addresses { get; set; }

            [JsonPropertyName("lastUpdated")]
            public DateTime LastUpdated { get; set; }
        }

        [JsonPropertyName("computerName")]
        public string ComputerName { get; set; }

        [JsonPropertyName("nicIp")]
        public IList<NicIPModel> NicIp { get; set; }
    }
}
