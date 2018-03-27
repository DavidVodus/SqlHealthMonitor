
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SqlHealthMonitor.DAL.Managers;
using System.Web;
using System.Web.Mvc;

namespace SqlHealthMonitor.Infrastructure
{
    abstract public class ControllerBase : Controller
    {
        protected ApplicationUserManager _userManager;

        private ApplicationRoleManager _roleManager;
        public virtual ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public virtual ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        [AllowAnonymous]
        public virtual ActionResult SaveLanguagePreference(string Language)
        {
            var currentUser = User.Identity.GetUserId();
            if (currentUser != null)
            {
                var user = UserManager.FindById(currentUser);
                user.Language = Language;
                UserManager.Update(user);
               // return RedirectToAction("index", new { lang = Language });
            }
            return RedirectToAction("index", new { lang = Language });
         
        }
    }
}