using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public class SqlDashBoardPageViewModel : PageViewModelBase
    {
      
        public List<WidgetViewModelBase> Widgets { get; set; }
      
    }
}
