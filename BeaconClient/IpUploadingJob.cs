using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BeaconClient
{
    [DisallowConcurrentExecution]
    public class IpUploadingJob : IJob
    {
        private readonly IIpUploadingService ipUploadingService;
        private readonly ILogger<IpUploadingJob> logger;

        public IpUploadingJob(IIpUploadingService ipUploadingService, ILogger<IpUploadingJob> logger)
        {
            this.ipUploadingService = ipUploadingService;
            this.logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            logger.LogInformation("Executing send ip job");
            // TODO: handle exception here
            // HttpRequestException
            return ipUploadingService.SendIpAsync();
        }
    }
}