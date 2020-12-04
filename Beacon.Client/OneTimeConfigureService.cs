using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Beacon.Client
{
    public class OneTimeConfigureService : IHostedService
    {
        private readonly IIpUploadingScheduler ipUploadingScheduler;
        private readonly IIpRetrievingService ipRetrievingService;
        private readonly Action<IList<NicIpInfo>> ipAddressChangeHandler;

        public OneTimeConfigureService(IIpUploadingScheduler ipUploadingScheduler, IIpRetrievingService ipRetrievingService)
        {
            this.ipUploadingScheduler = ipUploadingScheduler;
            this.ipRetrievingService = ipRetrievingService;
            ipAddressChangeHandler = nicInfoList => ipUploadingScheduler.TriggerNow();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ipUploadingScheduler.CreateJob();
            ipUploadingScheduler.SetSchedule("0 0 * * * ?"); // every hour
            ipUploadingScheduler.TriggerNow();

            ipRetrievingService.IpAddressChanged += ipAddressChangeHandler;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ipRetrievingService.IpAddressChanged -= ipAddressChangeHandler;
            ipUploadingScheduler.Unschedule();
            return Task.CompletedTask;
        }
    }
}
