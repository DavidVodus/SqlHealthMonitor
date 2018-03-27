using System.Web;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public abstract class PageViewModelBase
    {
        protected PageViewModelBase()
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            if (routeValues != null)
            {
                if (routeValues.ContainsKey("action"))
                {
                    StartActionName = string.IsNullOrEmpty(StartActionName) ?routeValues["action"].ToString(): StartActionName;
                }
                if (routeValues.ContainsKey("controller"))
                {
                    ControllerName = string.IsNullOrEmpty(ControllerName)?routeValues["controller"].ToString() : ControllerName;
                    PageName = string.IsNullOrEmpty(PageName) ? routeValues["controller"].ToString() : PageName;
                }
               
            }
        }
        public int PageId { get; set; }
        public string ControllerName { get; set; }
        public string PageName { get; set; }
        public string StartActionName { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
