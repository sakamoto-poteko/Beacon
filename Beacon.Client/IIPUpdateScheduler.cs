namespace Beacon.Client
{
    public interface IIPUpdateScheduler
    {
        void CreateJob();

        void SetSchedule(string cron);

        void Unschedule();

        void TriggerNow();
    }
}
