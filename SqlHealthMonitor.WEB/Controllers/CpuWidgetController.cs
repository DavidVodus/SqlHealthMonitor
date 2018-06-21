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
    public class CpuWidgetController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private ICpuService _cpuBaseService;
        private IWidgetService _widgetService;
        private ISqlServerDataService _sqlServerDataService;
        public CpuWidgetController
            (ICpuService cpuBaseService, IWidgetService widgetService, ISqlServerDataService sqlServerDataService)

        {
            _sqlServerDataService = sqlServerDataService;
            _widgetService = widgetService;
            _cpuBaseService = cpuBaseService;

        }

        // GET: SqlDashBoard

        public ActionResult Index(CpuPageViewModel model)
        {
            return View(model);
        }



        public ActionResult Settings(int id)
        {
            var currentUserId = User.Identity.GetUserId();

            CpuWidgetViewModel cpuWidget = _widgetService.Read<CpuWidgetViewModel, CpuWidget>(x => x.WidgetId == id
              && x.ApplicationUserId == currentUserId).SingleOrDefault();
         
            return View("Settings", "_Layout",
             new CpuSettingsPageViewModel { Widget = cpuWidget});
        }
        [HttpPost]
        public ActionResult Settings(CpuSettingsPageViewModel model)
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
           new CpuPageViewModel { Error = ex });
            }
        }

        /// <summary>
        /// Get Sql records of CPU utilization and transform them into JSON
        /// </summary>
        /// <param name="sqlServerDataId"></param>
        /// <returns></returns>
        public ContentResult Read(int sqlServerDataId,int NumberOfRecords)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                return JsonHelper.JsonOk(_cpuBaseService.Get(sqlServerDataId, currentUserId,NumberOfRecords));
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }

    }
}