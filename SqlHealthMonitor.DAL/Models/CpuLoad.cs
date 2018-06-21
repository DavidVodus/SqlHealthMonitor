using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.DAL.Models
{
    public class CpuLoad
    {
        public int SqlServer { get; set; }
        public int Others { get; set; }
        public DateTime? EventTime { get; set; }
      
    }
}
