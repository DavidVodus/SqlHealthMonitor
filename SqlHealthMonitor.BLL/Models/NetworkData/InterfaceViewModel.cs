namespace SqlHealthMonitor.BLL.Models.NetworkData
{
   public class InterfaceViewModel
    {
        public string Duplex { get; set; }
        public int InterfaceId { get; set; }
        public string InterfaceVlan { get; set; }
        public string Mistnost { get; set; }
        public string ModInterface { get; set; }
        public string Name { get; set; }
        public string OrgJednotka { get; set; }
        public string Patro { get; set; }
        public string PosilaOdSwitche { get; set; }
        public string Poznamka { get; set; }

        public string RawDescription { get; set; }
        public string Rozdvojka { get; set; }
        public string ShutDown { get; set; }
        public string Speed { get; set; }
        public string Spravce { get; set; }
        public string StickyMacAdress { get; set; }
        public int? SwitchId { get; set; }
        public string TypZarizeni { get; set; }
        public string Uzivatel { get; set; }
        public string WorkStationHostname { get; set; }
        public string Zamceno { get; set; }
        public string Zasuvka { get; set; }
    }
}
