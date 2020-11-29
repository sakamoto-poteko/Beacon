using Quartz;

namespace BeaconClient
{
    public class IpUploadingScheduler : IIpUploadingScheduler
    {
        private readonly IScheduler scheduler;
        private readonly JobKey uploadingJobKey;
        private readonly TriggerKey uploadingTriggerKey;

        public IpUploadingScheduler(ISchedulerFactory factory)
        {
            scheduler = factory.GetScheduler().Result;
            uploadingJobKey = new JobKey(IpUploadSchedulerConstants.JobId, IpUploadSchedulerConstants.JobGroup);
            uploadingTriggerKey = new TriggerKey(IpUploadSchedulerConstants.TriggerId);
        }

        public void CreateJob()
        {
            var jobdetail = JobBuilder.Create<IpUploadingJob>().WithIdentity(uploadingJobKey).StoreDurably().Build();
            scheduler.AddJob(jobdetail, true);
        }

        public void SetSchedule(string cron)
        {
            scheduler.UnscheduleJob(uploadingTriggerKey);
            var trigger = TriggerBuilder
                .Create()
                .ForJob(uploadingJobKey)
                .StartNow()
                .WithIdentity(uploadingTriggerKey)
                .WithCronSchedule(cron)
                .Build();
            scheduler.ScheduleJob(trigger);
        }

        public void TriggerNow()
        {
            scheduler.TriggerJob(uploadingJobKey);
        }
    }
}
