using Spionshop_API2.Models;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Spionshop_API2.Controllers
{
    public class HomeController : Controller
    {
        private SpionshopEntities db = new SpionshopEntities();

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var categories = db.Categories.ToList();
            //var categories = db.Artikels;

            return View(categories);
        }


        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
