using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Beacon.Client.Exceptions;
using Beacon.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Beacon.Client
{
    public class IpUploadingService : IIpUploadingService
    {
        private readonly HttpClient httpClient;
        private readonly IIpRetrievingService ipRetrievingService;
        private readonly ILogger<IpUploadingService> logger;
        private readonly string computerName;

        public IpUploadingService(HttpClient httpClient, IIpRetrievingService ipRetrievingService, ILogger<IpUploadingService> logger)
        {
            this.httpClient = httpClient;
            this.ipRetrievingService = ipRetrievingService;
            this.logger = logger;

            computerName = Environment.MachineName;
            logger.LogInformation("Computer name is {computerName}", computerName);
        }

        public async Task SendIpAsync(string computerName, IList<NicIpInfo> nicIpInfo)
        {
            if (nicIpInfo.Count == 0)
            {
                logger.LogInformation("No IP available on computer {computerName}", computerName);
            }

            var model = new UpdateIpRequestModel
            {
                ComputerName = computerName,
                NicIp = nicIpInfo.Select(info => new NicIpModelForUpdate
                {
                    Id = info.Id,
                    Name = info.NicName,
                    Addresses = info.Address.Select(addr => addr.ToString()).ToList()
                }).ToList()
            };

            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8,
                    "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("api/ip", content);
                try
                {
                    response.EnsureSuccessStatusCode();
                    logger.LogInformation("successfully updated IP. Total {ipCount} entries.", nicIpInfo.Count);
                }
                catch (HttpRequestException exception)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var message =
                        $"HTTP request failed.\nStatus code: {response.StatusCode}.\nMessage: {responseString}";
                    throw new UpdateIpException(message, exception);
                }
            }
            catch (TaskCanceledException exception)
            {
                throw new UpdateIpException("HTTP operation timed out", exception);
            }
            catch (AuthorizationTokenException exception)
            {
                throw new UpdateIpException("failed to retrieve authorization token", exception);
            }
        }

        public Task SendIpAsync()
        {
            var myIp = ipRetrievingService.GetIpForAllInterfaces();
            return SendIpAsync(computerName, myIp);
        }
    }
}
