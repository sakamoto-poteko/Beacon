namespace Beacon.Client
{
    public interface IIpUploadingScheduler
    {
        void CreateJob();

        void SetSchedule(string cron);

        void TriggerNow();
    }
}
