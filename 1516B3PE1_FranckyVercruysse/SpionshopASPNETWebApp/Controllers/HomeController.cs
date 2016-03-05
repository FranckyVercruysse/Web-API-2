using System.Web.Mvc;

namespace SpionshopASPNETWebApp.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
