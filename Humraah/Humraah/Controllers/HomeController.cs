using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humraah.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Humraah is about";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Humraah's developers.";

            return View();
        }
    }
}