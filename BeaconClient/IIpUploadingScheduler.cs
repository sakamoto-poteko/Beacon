using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaconClient
{
    public interface IIpUploadingScheduler
    {
        void CreateJob();

        void SetSchedule(string cron);

        void TriggerNow();
    }
}
