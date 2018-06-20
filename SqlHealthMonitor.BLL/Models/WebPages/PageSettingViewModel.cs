using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public class PageSettingViewModel: PageViewModelBase
    {
        public TimeSpan CacheDuration { get; set; }
        public string PageType { get; set; }
    }
}
