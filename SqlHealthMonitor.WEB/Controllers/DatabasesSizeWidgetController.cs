using AutoMapper;
using Microsoft.AspNet.Identity;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.BLL.Services;
using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.DAL.Models.Widgets;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using SqlHealthMonitor.Helpers;
using System.Web;
using SqlHealthMonitor.DAL.Managers;
using Microsoft.AspNet.Identity.Owin;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class DatabasesSizeWidgetController : SqlHealthMonitor.Infrastructure.ControllerBase
    {

        private IWidgetService _widgetService;
        private ISqlServerDataService _sqlServerDataService;
        private IDatabasesSizeService _databasesService;
        public DatabasesSizeWidgetController
            (IDatabasesSizeService databasesService, IWidgetService widgetService, ISqlServerDataService sqlServerDataService)

        {
            _sqlServerDataService = sqlServerDataService;
            _widgetService = widgetService;
            _databasesService = databasesService;

        }



        public ActionResult Index(DatabasesSizePageViewModel model)
        {
            return View(model);
        }

        public ActionResult Read(int sqlServerDataId, List<int> databaseIds, string jtSorting = null)
        {
           
            try
            {

                var currentUserId = User.Identity.GetUserId();
                return JsonHelper.JsonOk(_databasesService.Get(sqlServerDataId, currentUserId, databaseIds,jtSorting));
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }

        public ActionResult Settings(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            DatabasesSizeWidgetViewModel databasesSizeWidget = (DatabasesSizeWidgetViewModel)_widgetService.Read(x => x.WidgetId == id
              && x.ApplicationUserId == currentUserId).SingleOrDefault();

            return View("Settings", "_Layout",
             new DatabasesSizeSettingsPageViewModel { Widget = databasesSizeWidget });
        }
        [HttpPost]
        public ActionResult Settings(DatabasesSizeSettingsPageViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserId = User.Identity.GetUserId();
                    _widgetService.Update(model.Widget, currentUserId);
                    //// If we got this far, something failed, redisplay form
                    return RedirectToAction("Widgets", "SqlDashBoard", new { Message = Resources.Global.Success });
                    //return View("Index", "_Layout",
                    //    new CpuPageViewModel { OkMessage = Resources.Global.Success });
                }
                else
                {
                    return View("Settings", "_Layout", model);
                }

            }
            catch (Exception ex)
            {
                return View("Settings", "_Layout",
           new DatabasesSizePageViewModel { Error = ex });
            }
        }


    }
}