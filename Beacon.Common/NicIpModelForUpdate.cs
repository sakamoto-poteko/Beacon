using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Beacon.Common
{
    public class NicIpModelForUpdate
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public IEnumerable<string> Addresses { get; set; }
    }

    public class NicIpModelForGet : NicIpModelForUpdate
    {
        public DateTime LastUpdated { get; set; }
    }
}
