using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.BLL.Services;
using System.Web.Mvc;


namespace SqlHealthMonitor.WEB.Controllers
{
    public class SqlDashBoardController :  SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private ICpuService _cpuBaseService;
        public SqlDashBoardController
            (ICpuService cpuBaseService)

        {
            _cpuBaseService = cpuBaseService;

        }

        // GET: SqlDashBoard
        public ActionResult Index()
        {
            return View("Index", "_Layout", new SqlDashBoardPageViewModel());
        }
        [Route("{lang}/{controllerName}/Settings/AddDbServer")]
        public ActionResult AddDbServer()
        {
            return View("Settings/AddDbServer", "Settings/_Layout", 
                new AddDbServerViewModel { StartActionName= "Settings/AddDbServer" });
        }
        // POST: 
        [Route("{lang}/{controllerName}/Settings/AddDbServer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void AddDbServer(AddDbServerViewModel model)
        {

        }
        [Route("{lang}/{controllerName}/Settings/RemoveDbServer")]
        public ActionResult RemoveDbServer()
        {
            return View("Settings/RemoveDbServer", "Settings/_Layout", 
                new AddDbServerViewModel { StartActionName = "Settings/RemoveDbServer" });
        }
      
     

        public ActionResult AddNewWidget()
        {
            return View("AddNewWidget", "_Layout", new SqlDashBoardPageViewModel());
        }
        public JsonResult CpuLoad()
        {
            return Json(_cpuBaseService.GetCpuUsage());
        }

    }
}