using System;

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using Microsoft.AspNet.Identity;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.DAL.Models.WebPages;
using Newtonsoft.Json;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using SqlHealthMonitor.DAL.Managers;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using SqlHealthMonitor.BLL.Services;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class ManagePreferenceUserController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
   
        private IPageService _pageService;

        // GET: ManagePreferenceUser

        public ManagePreferenceUserController(IPageService pageService)
        {

            _pageService = pageService;
          

        }
     

        public ActionResult Index()
        {
          
            if (Request.IsAjaxRequest())
            {
                return PartialView("Index",new ManagePreferenceUserViewModel());
            }
            else
            {
                return View("Index", "_Layout", new ManagePreferenceUserViewModel());
            }

        }
       
        public ActionResult CreatePages()
        {
            var model = new ManagePreferenceUserViewModel();
            try
            {
                var container = (HttpContext.ApplicationInstance as IContainerAccessor).Container;
                BLL.Helpers.Database.CreateViews(container.Resolve<DbContext>());

            }
            catch (System.Exception e)
            {
                return Json(new { Result = "ERROR", Message = e.Message });
            }
            return Json(new { Result = "OK", Message= Resources.Global.Success });
        }
       
        [Route("{lang}/managePreferenceUser/{param:regex(.*)}/{controllerName}/{startActionName}")]
        public ActionResult NotImplemented(string controllerName,string startActionName)
        {
            Exception exception=new ApplicationException(controllerName+"/"+ startActionName+"  Not Implemented Yet");

            if (Request.IsAjaxRequest())
            {
                return PartialView("Error", new ErrorPageViewModel(exception)
              );
            }
            else
            {
                return View("Error", "_Layout",new  ErrorPageViewModel(exception)
             );
            }

        }
      
        public ActionResult ReadPages()
        {
            var currentUserId = User.Identity.GetUserId();
           var result = _pageService.ReadPages(currentUserId);
            return Json(new { Result = "OK", Records = result });


        }
    }

}