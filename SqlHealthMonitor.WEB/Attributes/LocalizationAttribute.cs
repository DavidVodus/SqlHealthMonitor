using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace SqlHealthMonitor.Attributes
{
    public class LocalizationAttribute : ActionFilterAttribute
    {
        private string _DefaultLanguage = "cs-CZ";

        public LocalizationAttribute(string defaultLanguage)
        {
            _DefaultLanguage = defaultLanguage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string lang = null;
            if ((string)filterContext.RouteData.Values["lang"] == null)
            {
                ApplicationUser user = null;
                var manager = HttpContext.Current.GetOwinContext().Get<ApplicationUserManager>();
                if (manager != null)
                {
                    var userId = HttpContext.Current.User.Identity.GetUserId();
                    if (userId != null)
                        user = manager.FindById(userId);
                }
                lang = user != null&& user.Language != null ? user.Language : _DefaultLanguage;
                filterContext.RouteData.Values.Add("lang", lang);
            }
            else
                lang = (string)filterContext.RouteData.Values["lang"];
                try
                {
                    Thread.CurrentThread.CurrentCulture =
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
                }
                catch (Exception e)
                {
                    throw new NotSupportedException(String.Format("ERROR: Invalid language code '{0}'.", lang));
                }
            
        }
    }
}