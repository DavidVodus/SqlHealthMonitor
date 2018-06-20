using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public class CpuSettingsPageViewModel :PageViewModelBase
    {
       
        public CpuWidgetViewModel Widget { get; set; }

    }
}
