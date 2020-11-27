using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
    public static class SubmitIpAddressFunction
    {
        [FunctionName("SubmitIpAddress")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] SubmitIpModel model,
            [Table(HostIpDataEntity.TableName, Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ClaimsPrincipal principal, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var userIdClaim = principal?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = userIdClaim?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                log.LogError($"request has invalid userId: {userId}");
                return new UnauthorizedResult();
            }

            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResult);

            if (!isValid)
            {
                return new BadRequestObjectResult(validationResult);
            }

            try
            {
                TableBatchOperation batchOperations = new TableBatchOperation();

                foreach (var nicInfo in model.NicIp)
                {
                    var entity = new HostIpDataEntity(userId, model.ComputerName, nicInfo.Id)
                    {
                        Addresses = nicInfo.Addresses,
                        Name = nicInfo.Name
                    };

                    TableOperation insertOp = TableOperation.InsertOrMerge(entity);
                    batchOperations.Add(insertOp);
                }
                TableBatchResult result = await cloudTable.ExecuteBatchAsync(batchOperations);
            }
            catch (StorageException e)
            {
                log.LogError(e.Message);
                throw;
            }

            var c = principal.Claims.Select(x => new { x.Type, x.Value });
            return new OkObjectResult(new { Claims = c, UserId = userId });

            //return new OkResult();
        }
    }
}
