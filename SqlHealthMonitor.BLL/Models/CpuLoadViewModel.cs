using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{
    public class CpuLoadViewModel
    {
        public int SqlServer { get; set; }
        public int Others { get; set; }
        public DateTime? EventTime { get; set; }
        public string EventTimeText { get { return EventTime == null ? "NULL" : EventTime.Value.ToShortTimeString(); } set { } }
    }
}
