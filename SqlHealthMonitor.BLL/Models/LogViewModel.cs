
using System;

namespace SqlHealthMonitor.BLL.Models
{
   public class LogViewModel
    {
        public string Exception { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public int LogId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
