using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Beacon.Common;
using Microsoft.Extensions.Hosting;

namespace Beacon.Client
{
    public class OneTimeConfigureService : IHostedService
    {
        private readonly IIPUpdateScheduler ipUploadingScheduler;
        private readonly IIPRetrievingService ipRetrievingService;
        private readonly Action<IList<NetworkInterfaceIPModel>> ipAddressChangeHandler;

        public OneTimeConfigureService(IIPUpdateScheduler ipUploadingScheduler, IIPRetrievingService ipRetrievingService)
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

            ipRetrievingService.IPAddressChanged += ipAddressChangeHandler;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ipRetrievingService.IPAddressChanged -= ipAddressChangeHandler;
            ipUploadingScheduler.Unschedule();
            return Task.CompletedTask;
        }
    }
}
