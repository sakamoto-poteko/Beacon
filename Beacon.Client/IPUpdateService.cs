using System;
using System.Threading.Tasks;
using Beacon.Common;
using Microsoft.Extensions.Logging;

namespace Beacon.Client
{
    public class IPUpdateService : IIPUpdateService
    {
        private readonly BeaconHttpClient httpClient;
        private readonly IIPRetrievingService ipRetrievingService;
        private readonly ILogger<IPUpdateService> logger;
        private readonly string computerName;

        public IPUpdateService(BeaconHttpClient httpClient, IIPRetrievingService ipRetrievingService, ILogger<IPUpdateService> logger)
        {
            this.httpClient = httpClient;
            this.ipRetrievingService = ipRetrievingService;
            this.logger = logger;

            computerName = Environment.MachineName;
            logger.LogInformation("Computer name is {computerName}", computerName);
        }

        public Task UpdateMyIPAsync()
        {
            var myIp = ipRetrievingService.GetIpForAllInterfaces();
            return httpClient.UpdateIPAsync(computerName, myIp);
        }
    }
}
