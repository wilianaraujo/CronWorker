using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronWorker.Configuration
{
    public interface IScheduleConfig<T>
    {
        IEnumerable<string> CronExpressions { get; set; }
        
        TimeZoneInfo TimeZone { get; set; }
    }
}
