using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Cosmos.Table;

namespace BeaconServer
{
    public class HostIpDataEntity : TableEntity
    {
        public class NicIpDataEntity
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public IList<string> Addresses { get; set; }
        }

        public const string TableName = "HostIpMap";

        public string UserId { get; set; }

        public string ComputerName { get; set; }

        public NicIpDataEntity NicIpData { get; set; }

        public HostIpDataEntity()
        {
        }

        public HostIpDataEntity(string userId, string computerName)
        {
            this.UserId = UserId;
            this.ComputerName = computerName;
            this.PartitionKey = GetPartitionKey(userId);
            this.RowKey = GetRowKey(userId, computerName);
        }

        public static string GetPartitionKey(string userId)
        {
            return userId.ToString();
        }

        public static string GetRowKey(string userId, string computerName)
        {
            return $"{userId}_{computerName}";
        }
    }
}
