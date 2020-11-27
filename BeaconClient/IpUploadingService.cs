using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Beacon.Common;
using Microsoft.Extensions.Logging;

namespace Beacon
{
    public class IpUploadingService : IIpUploadingService
    {
        private readonly HttpClient httpClient;
        private readonly IIpRetrivingService ipRetrivingService;
        private readonly ILogger<IpUploadingService> logger;
        private readonly string computerName;

        public IpUploadingService(HttpClient httpClient, IIpRetrivingService ipRetrivingService, ILogger<IpUploadingService> logger)
        {
            this.httpClient = httpClient;
            this.ipRetrivingService = ipRetrivingService;
            this.logger = logger;
            this.ipRetrivingService.IpAddressChanged += IpRetrivingService_IpAddressChanged;

            this.computerName = Environment.MachineName;
        }

        public async Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo)
        {
            if (nicIpInfo.Count == 0)
            {
                this.logger.LogInformation($"No IP available on computer {computerName}");
            }

            var model = new SubmitIpModel
            {
                ComputerName = computerName,
                NicIp = nicIpInfo.Select(info => new NicIpModel
                {
                    Id = info.Id,
                    Name = info.NicName,
                    Addresses = info.Address.Select(addr => addr.ToString()).ToList()
                }).ToList()
            };

            var response = await this.httpClient.PostAsJsonAsync("http://localhost:7071/api/SubmitIpAddress", model);

            if (response.IsSuccessStatusCode)
            {
                this.logger.LogInformation($"successfully updated IP for {computerName}. Total {nicIpInfo.Count} entrie(s)");
            }
            else
            {
                var responseString = await response.Content.ReadAsStringAsync();
                this.logger.LogError($"failed to update IP for {computerName}. Status code: {response.StatusCode}. Message: {responseString}");
            }
        }

        public Task SendIpAsync()
        {
            var myIp = this.ipRetrivingService.GetIpForAllNics();
            return SendIpAsync(this.computerName, myIp);
        }

        private async void IpRetrivingService_IpAddressChanged(IList<NicIpInfo> ipInfoList)
        {
            await SendIpAsync(this.computerName, ipInfoList);
        }
    }
}
