using AutoMapper;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.BLL.Services;
using SqlHealthMonitor.Helpers;

using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class SqlDashBoardController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private IWidgetService _widgetService;
        public SqlDashBoardController
            ( IWidgetService widgetService)

        {
            _widgetService = widgetService;


        }

        public ActionResult Index()
        {
            return RedirectToAction("Widgets");

        }

        public ActionResult Settings()
        {
            return RedirectToAction("Index", "SqlServerGrid");
        }


        public ActionResult Widgets(string Message)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();

                var widgets = _widgetService.Read(x => x.ApplicationUserId == currentUserId);

                return View("Widgets", "_Layout", new SqlDashBoardPageViewModel { Widgets = widgets ,OkMessage=Message});
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }
        public ActionResult WidgetsSettingsUpdate(WidgetViewModelBase[] models)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                foreach (var item in models)
                {
                    _widgetService.Update(item,currentUserId);
                }

                return JsonHelper.JsonOk(null,Resources.Global.Success);

            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }


        }
    }
}