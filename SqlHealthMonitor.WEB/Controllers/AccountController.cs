using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web.Mvc;
using SqlHealthMonitor.BLL.Models.Identity;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class AccountController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private ApplicationSignInManager _signInManager;

       

        public AccountController()
        {
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
         return   RedirectToAction("login");
        }
            public AccountController(ApplicationSignInManager signInManager)
        {
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

      
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var user = UserManager.FindByName("Admin");
            if (user == null)
            {
                BLL.Helpers.Database.CreateAdmin();
            }
         
            ViewBag.ReturnUrl = returnUrl;
            return View("Login", "_Layout",new LoginViewModel());
         
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", "_Layout",model);
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                {
                    if (HttpContext.Request.RequestContext.RouteData.Values["lang"] != null)
                    {
                        var lang = HttpContext.Request.RequestContext.RouteData.Values["lang"];
                        HttpContext.Request.RequestContext.RouteData.Values["lang"] = null;
                        if (returnUrl != null)
                            return RedirectToLocal(returnUrl.Replace("/" + lang, ""));
                        else
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    return RedirectToLocal(returnUrl);
                }
                case SignInStatus.LockedOut:
                    return View("Lockout",new ErrorPageViewModel(null));
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Resources.Global.Invalidlogin);
                    return View("Login", "_Layout", model);
            }
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            return View("Register", "_Layout", new RegisterViewModel());
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                { UserName = model.UserName, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return View("ConfirmEmail", "_Layout");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View("Register", "_Layout",model);
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

    
    }
}