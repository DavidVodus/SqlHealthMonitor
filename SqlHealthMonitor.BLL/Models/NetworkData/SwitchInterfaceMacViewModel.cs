namespace SqlHealthMonitor.BLL.Models.NetworkData
{
  public  class SwitchInterfaceMacViewModel
    {
        public InterfaceViewModel Interface { get; set; }
        public MacTableRecordViewModel MacRecord { get; set; }
        public SwitchViewModel Switch { get; set; }
    }
}
