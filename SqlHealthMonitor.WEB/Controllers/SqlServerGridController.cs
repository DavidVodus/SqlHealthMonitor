using AutoMapper;
using Microsoft.AspNet.Identity;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.BLL.Services;
using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.Helpers;
using System;
using System.Web.Mvc;
namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class SqlServerGridController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private ICpuService _cpuBaseService;
        private ISqlServerDataService _sqlServerDataService;
        public SqlServerGridController
            (ICpuService cpuBaseService, ISqlServerDataService sqlServerDataService)

        {
            _sqlServerDataService = sqlServerDataService;
            _cpuBaseService = cpuBaseService;

        }

        // GET: SqlDashBoard
        public ActionResult Index()
        {
        
            return View("Index", "_Layout",
                new SqlDashBoardPageViewModel());
        }
    
     
        // POST: 
        public ContentResult Read()
        {
            var currentUserId = User.Identity.GetUserId();
            var result = _sqlServerDataService.Read(x => x.ApplicationUserId == currentUserId);
            return JsonHelper.JsonOk(result);

        }
        [HttpPost]
        public ActionResult Create(SqlServerDataViewModel sqlServer)
        {
            var currentUserId = User.Identity.GetUserId();
            sqlServer.ApplicationUserId = currentUserId;
            try
            {
                if (!ModelState.IsValid)
                {
                    return JsonHelper.JsonError(Resources.Global.UnvalidForm);
                  
                }

                var addedSqlServer = _sqlServerDataService.Create(sqlServer);
                return Json(new { Result = "OK", Record = addedSqlServer });
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }
        [HttpPost]
        public ContentResult Delete(int sqlServerDataId)
        {

            try
            {
                var currentUserId = User.Identity.GetUserId();
                _sqlServerDataService.Delete(sqlServerDataId, currentUserId);
                return JsonHelper.JsonOk();
             
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }
        [HttpPost]
        public ContentResult Update(SqlServerDataViewModel sqlServer)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                if (!ModelState.IsValid)
                {
                    return JsonHelper.JsonError(Resources.Global.UnvalidForm);

                }
                _sqlServerDataService.Update(sqlServer, currentUserId);
                return JsonHelper.JsonOk();
            }
            catch (Exception ex)
            {
                return JsonHelper.JsonError(ex.Message);
            }
        }

       

    }
}