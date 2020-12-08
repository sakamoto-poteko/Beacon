using Quartz;

namespace Beacon.Client
{
    public class IPUpdateScheduler : IIPUpdateScheduler
    {
        private readonly IScheduler scheduler;
        private readonly JobKey uploadingJobKey;
        private readonly TriggerKey uploadingTriggerKey;

        public IPUpdateScheduler(ISchedulerFactory factory)
        {
            scheduler = factory.GetScheduler().Result;
            uploadingJobKey = new JobKey(IpUploadSchedulerConstants.JobId, IpUploadSchedulerConstants.JobGroup);
            uploadingTriggerKey = new TriggerKey(IpUploadSchedulerConstants.TriggerId);
        }

        public void CreateJob()
        {
            var jobdetail = JobBuilder.Create<IPUpdateJob>().WithIdentity(uploadingJobKey).StoreDurably().Build();
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
                .WithCronSchedule(cron, conf => conf.WithMisfireHandlingInstructionIgnoreMisfires())
                .Build();
            scheduler.ScheduleJob(trigger);
        }

        public void Unschedule()
        {
            scheduler.UnscheduleJob(uploadingTriggerKey);
        }

        public void TriggerNow()
        {
            scheduler.TriggerJob(uploadingJobKey);
        }
    }
}
