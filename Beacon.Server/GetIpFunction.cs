using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Beacon.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Beacon.Server
{
    public static class GetIpFunction
    {
        [FunctionName("GetIp")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            [Table(HostIpDataEntity.TableName, Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ClaimsPrincipal principal,
            ILogger log)
        {
            var userIdClaim = principal?.FindFirst(Claims.ObjectId);
            var userId = userIdClaim?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                log.LogError("request has invalid userId {UserId}", userId);
                return new UnauthorizedResult();
            }

            log.LogTrace("IP retrieve request received");

            string queryCondition = TableQuery.GenerateFilterCondition(nameof(HostIpDataEntity.PartitionKey), QueryComparisons.Equal, HostIpDataEntity.GetPartitionKey(userId));
            TableQuery<HostIpDataEntity> query = new TableQuery<HostIpDataEntity>().Where(queryCondition);

            IEnumerable<HostIpDataEntity> queryResult = cloudTable.ExecuteQuery<HostIpDataEntity>(query);

            var response = queryResult.GroupBy(result => result.ComputerName).Select(group =>
                new GetIpResponseModel
                {
                    ComputerName = group.Key,
                    NicIp = group.Select(hostIpDataEntity => new NicIpModelForGet
                    {
                        Name = hostIpDataEntity.NicName,
                        Id = hostIpDataEntity.NicId,
                        Addresses = hostIpDataEntity.Addresses,
                        LastUpdated = hostIpDataEntity.Timestamp.UtcDateTime
                    }).ToList()
                }
            );

            return new OkObjectResult(response);
        }
    }
}
