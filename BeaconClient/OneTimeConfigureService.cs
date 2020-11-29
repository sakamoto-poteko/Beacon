using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace BeaconClient
{
    public class OneTimeConfigureService : IHostedService
    {
        private readonly IIpUploadingScheduler ipUploadingScheduler;

        public OneTimeConfigureService(IIpUploadingScheduler ipUploadingScheduler)
        {
            this.ipUploadingScheduler = ipUploadingScheduler;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ipUploadingScheduler.CreateJob();
            ipUploadingScheduler.SetSchedule("0 * * * * ?"); // every second
            ipUploadingScheduler.TriggerNow();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
