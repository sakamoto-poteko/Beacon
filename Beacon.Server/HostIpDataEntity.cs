using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;

namespace Beacon.Server
{
    public class HostIpDataEntity : TableEntity
    {
        public const string TableName = "HostIpMap";

        public string Name { get; set; }

        public string UserId { get; set; }

        public string ComputerName { get; set; }

        [IgnoreProperty]
        public IList<string> Addresses
        {
            get => DeserializeAddresses(AddressesList);

            set => AddressesList = SerializeAddress(value);
        }

        public string AddressesList { get; set; }

        private string SerializeAddress(IList<string> addresses)
        {
            return JsonConvert.SerializeObject(addresses);
        }

        private IList<string> DeserializeAddresses(string addressesList)
        {
            return JsonConvert.DeserializeObject<IList<string>>(addressesList);
        }

        public HostIpDataEntity()
        {
        }

        public HostIpDataEntity(string userId, string computerName, string nicId)
        {
            this.UserId = UserId;
            this.ComputerName = computerName;
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
