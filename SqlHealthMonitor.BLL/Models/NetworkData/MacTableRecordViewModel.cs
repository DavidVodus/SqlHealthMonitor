using System;
using System.ComponentModel;
using SqlHealthMonitor.DAL.Managers;

namespace SqlHealthMonitor.BLL.Models.NetworkData
{
   public class MacTableRecordViewModel
    {
        [DataBaseType(typeof(Byte[]))]
        public string MacTableRecordMacAdress { get; set; }

        public string MacTableRecordVlan { get; set; }

        [DisplayName("Název Portu")]
        public string Port { get; set; }

        [DisplayName("Patří ke switchy")]
        public string SwitchHostName { get; set; }

        public DateTime? TimeStampMacTableRecord { get; set; }
    }
}
