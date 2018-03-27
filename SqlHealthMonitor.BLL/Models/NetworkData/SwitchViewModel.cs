using System;

namespace SqlHealthMonitor.BLL.Models.NetworkData
{
   public class SwitchViewModel
    {
        public string HostName { get; set; }
        public int SwitchId { get; set; }
        public string SwitchIp { get; set; }
        public DateTime SwitchTimeStamp { get; set; }
    }
}
