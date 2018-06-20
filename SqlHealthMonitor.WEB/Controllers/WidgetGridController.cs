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

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class WidgetGridController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private ICpuService _cpuBaseService;
        private IWidgetService _widgetService;
        private ISqlServerDataService _sqlServerDataService;
        public WidgetGridController
            (ICpuService cpuBaseService, IWidgetService widgetService, ISqlServerDataService sqlServerDataService)

        {
            _sqlServerDataService = sqlServerDataService;
            _widgetService = widgetService;
            _cpuBaseService = cpuBaseService;

        }

        // GET: SqlDashBoard
     
        public ActionResult Index()
        {
         
            var currentUserId = User.Identity.GetUserId();
            List<SqlServerDataViewModel> SqlServers =
                 _sqlServerDataService.Read<SqlServerDataViewModel>(x => x.ApplicationUserId == currentUserId);
            return View("Index", "_Layout",
                new WidgetGridPageViewModel {SqlServers= SqlServers });
        }
   
        // POST: 
        public ContentResult Read()
        {
            var currentUserId = User.Identity.GetUserId();
            var result = _widgetService.Read(x => x.ApplicationUserId == currentUserId);
            return JsonHelper.JsonOk(result);
        

        }
        [HttpPost]
        public ActionResult Create(WidgetViewModelBase widget)
        {
            var currentUserId = User.Identity.GetUserId();
            widget.ApplicationUserId = currentUserId;
            try
            {
                if (!ModelState.IsValid)
                {
                    return JsonHelper.JsonError(Resources.Global.UnvalidForm);
                }

                var addedSqlServer = _widgetService.Create(widget);
                return Json(new { Result = "OK", Record = addedSqlServer, Message = Resources.Global.Success });
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }
        [HttpPost]
        public ContentResult Delete(int widgetId)
        {

            try
            {
                var currentUserId = User.Identity.GetUserId();
                _widgetService.Delete(widgetId, currentUserId);
                return JsonHelper.JsonOk(Resources.Global.Success);
          
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }
        [HttpPost]
        public ContentResult Update(WidgetViewModelBase widgetView, string userId)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                _widgetService.Update(widgetView, currentUserId);
                return JsonHelper.JsonOk(Resources.Global.Success);
              
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
         
        }



    }
}