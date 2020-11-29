using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Beacon.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Beacon.Server
{
    public static class SubmitIpAddressFunction
    {
        [FunctionName("SubmitIpAddress")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            [Table(HostIpDataEntity.TableName, Connection = "AzureWebJobsStorage")] CloudTable cloudTable,
            ClaimsPrincipal principal, ILogger log)
        {
            var userIdClaim = principal?.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier");
            var userId = userIdClaim?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                log.LogError("request has invalid userId {UserId}", userId);
                return new UnauthorizedResult();
            }

            log.LogTrace("IP update request received");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            SubmitIpModel model;

            try
            {
                model = JsonConvert.DeserializeObject<SubmitIpModel>(requestBody);
            }
            catch (JsonException)
            {
                log.LogError("IP update request has invalid payload");
                return new BadRequestResult();
            }

            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(model, new ValidationContext(model), validationResult);

            if (!isValid)
            {
                log.LogError("IP update request has invalid model");
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

            log.LogTrace("successfully updated IP for computer {ComputerName}, {Entries} entries updated", model.ComputerName, model.NicIp.Count);

            return new OkResult();
        }
    }
}
