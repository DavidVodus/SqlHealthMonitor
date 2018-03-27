using System;

namespace SqlHealthMonitor.BLL.Models.NetworkData
{
    public class MaxSwitchesTimeStampViewModel
    {
        public string HostName { get; set; }
        public DateTime MaxTimeStamp { get; set; }
        public int SwitchId { get; set; }
    }
}
