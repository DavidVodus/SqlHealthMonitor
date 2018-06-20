using System.Web.Mvc;
using System.Web.Routing;

namespace SqlHealthMonitor
{
    public class RouteConfig
    {
      
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            //          routes.MapRoute(
            //    "CatchAll",
            //    "{*url}",
            //    new { controller = "Log", action = "Index" }
            //);
            routes.MapRoute(
 name: "DefaultLocalized",
 url: "{lang}/{controller}/{action}/{id}",
 constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },   // en or en-US
 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional , rootController = UrlParameter.Optional }
);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
          

        }
    }
}
