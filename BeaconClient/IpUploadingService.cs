using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Beacon.Common;
using BeaconClient.Exceptions;
using BeaconClient.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BeaconClient
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

            computerName = Environment.MachineName;
            logger.LogInformation("Computer name is {computerName}", computerName);
        }

        public async Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo)
        {
            if (nicIpInfo.Count == 0)
            {
                logger.LogInformation("No IP available on computer {computerName}", computerName);
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

            try
            {
                HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/SubmitIpAddress", model);
                try
                {
                    response.EnsureSuccessStatusCode();
                    logger.LogInformation("successfully updated IP. Total {ipCount} entrie(s)", nicIpInfo.Count);
                }
                catch (HttpRequestException)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    logger.LogError("failed to update IP. Status code: {statusCode}. Message: {responseString}",
                        response.StatusCode, responseString);
                }
            }
            catch (AuthorizationTokenException exception)
            {
                logger.LogError("failed to retrieve authorization token: {message}", exception.Message);
            }
        }

        public Task SendIpAsync()
        {
            var myIp = ipRetrivingService.GetIpForAllNics();
            return SendIpAsync(computerName, myIp);
        }

        private async void IpRetrivingService_IpAddressChanged(IList<NicIpInfo> ipInfoList)
        {
            await SendIpAsync(computerName, ipInfoList);
        }
    }
}
