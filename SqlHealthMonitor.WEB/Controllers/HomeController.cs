using Newtonsoft.Json;
using System.Web.Mvc;

using SqlHealthMonitor.BLL.Models.WebPages;
using Common;


namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize]
    public class HomeController : SqlHealthMonitor.Infrastructure.ControllerBase
    {


        public HomeController()
        {
     
        }
        public ActionResult Index()
        {
           return View("Index", "_Layout", new HomePageViewModel());

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