using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Beacon.Client.Exceptions;
using Beacon.Common;

namespace Beacon.Client
{
    public class BeaconHttpClient
    {
        private readonly HttpClient httpClient;

        public BeaconHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task UpdateIPAsync(string computerName, IEnumerable<NetworkInterfaceIPModel> interfaces)
        {
            if (interfaces.Count() == 0)
            {
                return;
            }

            var model = new UpdateIPRequestModel
            {
                ComputerName = computerName,
                NicIp = interfaces.Select(info => new UpdateIPRequestModel.NicIPModel
                {
                    Id = info.InterfaceId,
                    Name = info.InterfaceName,
                    Addresses = info.IPAddresses.Select(addr => addr.ToString()).ToList()
                }).ToList()
            };

            try
            {
                StringContent content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8,
                    "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("api/ip", content);

                if (!response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var message =
                        $"HTTP request failed.\nStatus code: {response.StatusCode}.\nMessage: {responseString}";
                    throw new UpdateIPException(message);
                }
            }
            catch (TaskCanceledException exception)
            {
                throw new UpdateIPException("HTTP operation timed out", exception);
            }
            catch (HttpRequestException exception)
            {
                throw new UpdateIPException("HTTP request failed", exception);
            }
        }


        public async Task<IEnumerable<(string computerName, IEnumerable<NetworkInterfaceIPModel> interafces)>> GetIPAsync()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("api/ip");
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var message =
                        $"HTTP request failed.\nStatus code: {response.StatusCode}.\nMessage: {responseString}";
                    throw new GetIPException(message);
                }
                else
                {
                    try
                    {
                        var result = JsonSerializer.Deserialize<IEnumerable<GetIPResponseModel>>(responseString);
                        return result.Select(computerEntry => (computerName: computerEntry.ComputerName,
                        interfaces: computerEntry.NicIp.Select(iface => new NetworkInterfaceIPModel
                        {
                            InterfaceId = iface.Id,
                            InterfaceName = iface.Name,
                            IPAddresses = iface.Addresses.Select(ip => IPAddress.Parse(ip))
                        })));
                    }
                    catch (JsonException exception)
                    {
                        throw new GetIPException($"Invalid server response:\n{responseString}", exception);
                    }
                }
            }
            catch (TaskCanceledException exception)
            {
                throw new GetIPException("HTTP operation timed out", exception);
            }
            catch (HttpRequestException exception)
            {
                throw new GetIPException("HTTP request failed", exception);
            }
        }
    }
}
