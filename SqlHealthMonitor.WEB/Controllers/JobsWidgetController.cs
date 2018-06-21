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
    public class JobsWidgetController : SqlHealthMonitor.Infrastructure.ControllerBase
    {

        private IWidgetService _widgetService;
        private ISqlServerDataService _sqlServerDataService;
        private IJobsService _jobsService;
        public JobsWidgetController
            (IJobsService jobsService, IWidgetService widgetService, ISqlServerDataService sqlServerDataService)

        {
            _sqlServerDataService = sqlServerDataService;
            _widgetService = widgetService;
            _jobsService = jobsService;

        }



        public ActionResult Index(JobsPageViewModel model)
        {
            return View(model);
        }

        public ActionResult Read(int sqlServerDataId,string jtSorting = null)
        {
            try
            {

                var currentUserId = User.Identity.GetUserId();
                return JsonHelper.JsonOk(_jobsService.Get(sqlServerDataId, currentUserId, jtSorting));
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }
      



    }
}