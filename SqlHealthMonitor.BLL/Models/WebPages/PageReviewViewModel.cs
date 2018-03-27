using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kendo.Mvc.UI.Html;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public class PageReviewViewModel: PageViewModelBase
    {
        public TimeSpan CacheDuration { get; set; }
        public string PageType { get; set; }
    }
}
