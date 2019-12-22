using Humraaah.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humraaah.Controllers
{
    public class HomeController : Controller
    {
        private HumraahContext db = new HumraahContext();

        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            if (user != null)
            {
                string strCurrentUserId = user.Id;
                User_ s = db.User_.Find(strCurrentUserId);
                if (s != null)
                {
                    ViewBag.Role = 1;
                }
                else
                {
                    Station s1 = db.Stations.Find(strCurrentUserId);
                    if (s1 != null)
                        ViewBag.Role = 2;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}