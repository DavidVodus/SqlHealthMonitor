using System;

namespace SqlHealthMonitor.BLL.Models.WebPages.Components
{
    public class GridColumnDefinitionViewModel
    {
        public int GridColumnDefinitionId { get; set; }
        public string Attribute { get; set; }
        public string BindName { get; set; }
        public String DateType { get; set; }
        public string DisplayName { get; set; }
        public bool IsPk { get; set; }
        public int PageId { get; set; }
        public int DateTimeDifference { get; set; }
        public bool Hiden { get; set; }
        public int Order { get; set; }

    }
}
