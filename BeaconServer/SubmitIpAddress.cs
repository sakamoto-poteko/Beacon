using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Beacon.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace BeaconServer
{
    public static class SubmitIpAddress
    {
        [FunctionName("SubmitIpAddress")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] SubmitIpModel model,
            [Table(HostIpDataEntity.TableName, Connection = "ConnectionStrings:AzureWebJobsStorage")] CloudTable cloudTable,
            ClaimsPrincipal principal, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userIdClaim?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new UnauthorizedResult();
            }
            try
            {
                var entity = new HostIpDataEntity(userId, model.ComputerName);
                
                TableOperation insertOp = TableOperation.InsertOrMerge(entity);
                TableResult result = await cloudTable.ExecuteAsync(insertOp);
            }
            catch (StorageException e)
            {
                log.LogError(e.Message);
                throw;
            }

            return new OkResult();
        }
    }
}
