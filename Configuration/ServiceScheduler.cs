using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronWorker.Configuration
{
    public class ServiceScheduler
    {
        public string TimeZone = "E. South America Standart Time";
        public IEnumerable<string> Sincronizacao { get; set;}
    }
}
