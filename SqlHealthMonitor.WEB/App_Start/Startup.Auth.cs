using System;
using System.Data.Entity;
using Castle.Windsor;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using Owin;
using System.Web.Mvc;
using System.Web;

namespace SqlHealthMonitor
{

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app, IWindsorContainer container)
        {
      
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(()=> container.Resolve<DbContext>());
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                 
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    
                    OnApplyRedirect = ApplyRedirect,
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, string>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                            // Need to add THIS line because we added the third type argument (int) above:
                            getUserIdCallback: claim => claim.GetUserId())
                }
            });
        }

            private static void ApplyRedirect(CookieApplyRedirectContext context)
        {

            UrlHelper _url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            //var loginUrl = context.RedirectUri.Insert(
            //    context.RedirectUri.IndexOf("/Account/Login"),
            //    "/" + "cs-CZ");
  
             String actionUri = _url.Action("Login", "Account", new { lang = "cs-CZ" });
            context.Response.Redirect(context.RedirectUri);
        }
    }
}