using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronWorker.Configuration
{
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public IEnumerable <string> CronExpressions { get; set; }
        public TimeZoneInfo TimeZone { get; set; }
    }
}
