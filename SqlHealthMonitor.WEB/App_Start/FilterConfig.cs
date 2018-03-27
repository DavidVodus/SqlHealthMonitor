using SqlHealthMonitor.Attributes;
using System.Threading;
using System.Web.Mvc;

namespace SqlHealthMonitor
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LocalizationAttribute(Thread.CurrentThread.CurrentCulture.Name), 0);
        }
    }
}
