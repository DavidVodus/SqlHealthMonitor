using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{
    public class SqlJobsViewModel
    {
        public string Jobname { get; set; }
        public string LastRunStatus { get; set; }
        public string LastRunStatusMessage { get; set; }
        public DateTime? LastRunDateTime { get; set; }
        public String LastRunDuration { get; set; }
        public DateTime? NextRunDateTime { get; set; }
    }
}
