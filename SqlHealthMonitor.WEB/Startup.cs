using Owin;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.Windsor;
using System;
using Common;

namespace SqlHealthMonitor
{
  
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
               
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var containerAccessor = HttpContext.Current.ApplicationInstance as IContainerAccessor;
            var container = containerAccessor.Container;
            ConfigureAuth(app,container);



        }
    }
}