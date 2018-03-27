namespace SqlHealthMonitor.BLL.Models.NetworkData
{
   public  class SwitchInterfaceMacProbeViewModel
    {
        public InterfaceViewModel Interface { get; set; }
        public MacTableRecordViewModel MacRecord { get; set; }
        public ProbeDataViewModel ProbeData { get; set; }
        public SwitchViewModel Switch { get; set; }
    }
}
