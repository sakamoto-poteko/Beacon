using System;
using System.Text;
using System.Threading.Tasks;
using Beacon.Client.Exceptions;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Beacon.Client
{
    [DisallowConcurrentExecution]
    public class IPUpdateJob : IJob
    {
        private readonly IIPUpdateService ipUploadingService;
        private readonly ILogger<IPUpdateJob> logger;

        public IPUpdateJob(IIPUpdateService ipUploadingService, ILogger<IPUpdateJob> logger)
        {
            this.ipUploadingService = ipUploadingService;
            this.logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("updating IP address...");
            try
            {
                await ipUploadingService.UpdateMyIPAsync();
            }
            catch (UpdateIPException exception)
            {
                logger.LogError("failed to update IP: {message}", exception.Message);
            }
            catch (Exception exception)
            {
                var message = GetExceptionMessage(exception);
                logger.LogError("unhandled error {message}", message);
            }
        }

        private string GetExceptionMessage(Exception exception)
        {
            var builder = new StringBuilder();

            AppendExceptionMessage(exception, builder);

            builder.AppendLine(exception.StackTrace);

            return builder.ToString();
        }

        private void AppendExceptionMessage(Exception exception, StringBuilder builder)
        {
            while (exception != null)
            {
                builder.Append(exception.GetType().FullName);
                builder.Append(": ");
                builder.AppendLine(exception.Message);
                exception = exception.InnerException;
            }
        }
    }
}