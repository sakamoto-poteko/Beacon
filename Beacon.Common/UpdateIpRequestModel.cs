using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Beacon.Common
{
    public class UpdateIpRequestModel
    {
        [Required]
        public string ComputerName { get; set; }

        [Required]
        public IList<NicIpModelForUpdate> NicIp { get; set; }
    }
}
