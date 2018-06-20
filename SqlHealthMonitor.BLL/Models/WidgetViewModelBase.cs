using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.DAL.Models.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models
{
   
    public class WidgetViewModelBase
    {
        public WidgetViewModelBase()
        {
            Order = -1;
        }
        public int WidgetId { get; set; }
        public string Name { get; set; }
        public string ApplicationUserId { get; set; }
        public WidgetType Type { get; set; }
        public long UpdateInterval { get; set; }
        public int Order { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int? SqlServerDataId { get; set; }

    }
}
