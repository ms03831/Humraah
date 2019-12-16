using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Humraah.DAL;
using Humraah.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Humraah.Controllers
{
    [Authorize(Roles = "User")]
    public class BookingsController : Controller
    {
        private HumraahContext db = new HumraahContext();
        // GET: Bookings
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var bookings = (from bk in db.Bookings
                           where bk.User__id.Equals(strCurrentUserId)
                           select bk);
            return View(bookings.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var booking = (from bk in db.Bookings
                             where bk.User__id.Equals(strCurrentUserId) && bk.idBooking == id
                             select bk).FirstOrDefault();
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking bk = db.Bookings.Find(id);
            db.Bookings.Remove(bk);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}


