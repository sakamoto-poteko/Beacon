using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Beacon.Common
{
    public class NicIpModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public IList<string> Addresses { get; set; }
    }

    public class SubmitIpModel
    {
        [Required]
        public string ComputerName { get; set; }

        [Required]
        public IList<NicIpModel> NicIp { get; set; }
    }
}
