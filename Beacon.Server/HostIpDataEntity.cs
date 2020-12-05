using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace Beacon.Server
{
    public class HostIpDataEntity : TableEntity
    {
        public const string TableName = "HostIpMap";

        public string NicName { get; set; }

        public string NicId { get; set; }

        public string UserId { get; set; }

        public string ComputerName { get; set; }

        [IgnoreProperty]
        public IEnumerable<string> Addresses
        {
            get => DeserializeAddresses(AddressesList);

            set => AddressesList = SerializeAddress(value);
        }

        public string AddressesList { get; set; }

        private string SerializeAddress(IEnumerable<string> addresses)
        {
            return JsonConvert.SerializeObject(addresses);
        }

        private IEnumerable<string> DeserializeAddresses(string addressesList)
        {
            return JsonConvert.DeserializeObject<IEnumerable<string>>(addressesList);
        }

        public HostIpDataEntity()
        {
        }

        public HostIpDataEntity(string userId, string computerName, string nicId, string nicName, IEnumerable<string> addresses)
        {
            this.UserId = UserId;
            this.ComputerName = computerName;
            this.NicId = nicId;
            this.NicName = nicName;
            this.Addresses = addresses;
            this.PartitionKey = GetPartitionKey(userId);
            this.RowKey = GetRowKey(userId, computerName, nicId);
        }

        public static string GetPartitionKey(string userId)
        {
            return userId.ToString();
        }

        public static string GetRowKey(string userId, string computerName, string nicId)
        {
            return $"{userId}_{computerName}_{nicId}";
        }
    }
}
