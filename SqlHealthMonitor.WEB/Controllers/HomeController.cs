using Newtonsoft.Json;
using System.Web.Mvc;

using SqlHealthMonitor.BLL.Models.WebPages;
using Common;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class HomeController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        private const int PAGE_ID = 3;

        public HomeController()
        {
     
        }

        public ActionResult Index()
        {
          
            if (Request.IsAjaxRequest())
            {
                return PartialView("Index",new HomePageViewModel());
            }
            else
            {
                return View("Index", "_Layout", new HomePageViewModel());
            }
        }

        public ActionResult test( string value)
        {
           // var macAdress = JObject.Parse(value);
          
              var res=   JsonConvert.DeserializeObject<byte[]>(value);
        
      
            var bytes = Common.MacHelper.ConvertBytesToMac(res);

            //Common.MacHelper.ConvertBytesToMac();
            return Json(new { SomeValue = bytes }, JsonRequestBehavior.AllowGet);
        }
    }
}