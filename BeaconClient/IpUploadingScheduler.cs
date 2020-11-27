using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Beacon;
using Microsoft.Extensions.Hosting;

namespace BeaconClient
{
    public class IpUploadingScheduler : BackgroundService, IIpUploadingScheduler
    {
        private readonly IIpUploadingService ipUploadingService;

        public IpUploadingScheduler(IIpUploadingService ipUploadingService)
        {
            this.ipUploadingService = ipUploadingService;
        }

        public void SetSchedule(string cron)
        {
            throw new NotImplementedException();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return this.ipUploadingService.SendIpAsync();
        }
    }
}
