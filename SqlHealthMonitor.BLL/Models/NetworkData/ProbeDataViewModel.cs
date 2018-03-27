using System;
using SqlHealthMonitor.DAL.Managers;

namespace SqlHealthMonitor.BLL.Models.NetworkData
{
   public  class ProbeDataViewModel
    {
        public DateTime EndTime { get; set; }
        public string ProbeIp { get; set; }

        [DataBaseType(typeof(Byte[]))]
        public string ProbeMacAdress { get; set; }

        public string SourceTable { get; set; }


        public DateTime StartTime { get; set; }
    }
}
