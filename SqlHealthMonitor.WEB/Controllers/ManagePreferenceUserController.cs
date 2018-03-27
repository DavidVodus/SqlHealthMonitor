using System;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Castle.Windsor;
using Microsoft.AspNet.Identity;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.BLL.Models.WebPages.Components;

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

                HandleErrorInfo error = new HandleErrorInfo(e, model.ControllerName, model.StartActionName);
                var errorJson = JsonConvert.SerializeObject(error);
                return Json(errorJson, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }
        [Route("{lang}/managePreferenceUser/GridPage/{controllerName}/{startActionName}")]
        public ActionResult GridPage(string controllerName, string startActionName)
        {

            if (Request.IsAjaxRequest())
            {
                return PartialView("GridPage", new ManagePreferenceUserViewModel
                { ControllerName = controllerName,StartActionName = startActionName });
            }
            else
            {
                return View("GridPage", "_Layout", new ManagePreferenceUserViewModel
                { ControllerName = controllerName, StartActionName = startActionName });
            }

        }
        [Route("{lang}/managePreferenceUser/SqlDashBoardPage/{controllerName}/{startActionName}")]
        public ActionResult SqlDashBoardPage(string controllerName, string startActionName)
        {
            return RedirectToAction("/Settings/AddDbServer", controllerName );
            //return RedirectToAction("Settings", controllerName, new { Action2 = "AddDbServer" });

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
        //public ActionResult ReadGridColumns([DataSourceRequest]DataSourceRequest request, string controllerName, string startActionName)
        //{
        //    var currentUserId = User.Identity.GetUserId();
        //    request.Sorts.Add(new Kendo.Mvc.SortDescriptor("DisplayName", System.ComponentModel.ListSortDirection.Ascending));
        //    DataSourceResult result = _gridPageService.GetGridPageColumns(controllerName, startActionName, currentUserId).ToDataSourceResult(request);
        //    return Json(result);

        //}
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UpdateGridColumns([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]
        //    IList<GridColumnDefinitionViewModel> columns)
        //{

        //    if (columns != null && ModelState.IsValid)
        //    {
        //        _gridPageService.UpdateGridColumns(columns);
        //    }

        //    return Json(columns.ToDataSourceResult(request, ModelState));
        //}
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult UpdateGridPage([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]
        //    IList<GridPageViewModel> pageModel)
        //{

        //    if (pageModel != null && ModelState.IsValid)
        //    {
              
        //        _gridPageService.UpdateAllPageProperty(pageModel.SingleOrDefault());
        //    }

        //    return Json(pageModel.ToDataSourceResult(request, ModelState));
        //}
        public ActionResult ReadPageProperties
            ([DataSourceRequest]DataSourceRequest request, string controllerName, string startActionName)
        {
            var currentUserId = User.Identity.GetUserId();
            DataSourceResult result = new[]
                    {_pageService.ReadPageProperties(currentUserId, controllerName, startActionName)}
                .ToDataSourceResult(request);
            return Json(result);

        }


        //public ActionResult DestroyPage([DataSourceRequest] DataSourceRequest request,GridPageViewModel pageModel)
        //{

        //    if (pageModel != null && ModelState.IsValid)
        //    {
        //       _pageService.DeletePage(pageModel);
        //    }
        //    return Json(new []{pageModel }.ToDataSourceResult(request, ModelState));
        //}
        public ActionResult ReadPages([DataSourceRequest]DataSourceRequest request)
        {

            var currentUserId = User.Identity.GetUserId();
            DataSourceResult result = _pageService.ReadPages(currentUserId).ToDataSourceResult(request);
            return Json(result);

        }
    }

}