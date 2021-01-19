using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Beacon.Common
{
    public class UpdateIPRequestModel
    {
        public class NicIPModel
        {
            [Required]
            public string Id { get; set; }

            [Required]
            public string Name { get; set; }

            [Required]
            public IEnumerable<string> Addresses { get; set; }
        }

        [Required]
        public string ComputerName { get; set; }

        [Required]
        public IList<NicIPModel> NicIp { get; set; }
    }
}
