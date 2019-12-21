using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Humraaah.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Humraaah.Controllers
{
    public class BookingsController : Controller
    {
        private HumraahContext db = new HumraahContext();

        // GET: Bookings
        [OutputCache(Duration = 20, VaryByParam = "*")]
        [Authorize(Roles = "User")]
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var bookings = db.Bookings.Include(b => b.Ambulance).Include(b => b.User_);
            var bks = from bk in bookings
                      where bk.User__id == strCurrentUserId
                      select bk;

            return View(bks.ToList());
        }

        [OutputCache(Duration = 20, VaryByParam = "*")]
        public PartialViewResult GetAll()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var bookings = db.Bookings.Include(b => b.Ambulance).Include(b => b.User_);
            var bks = from bk in bookings
                      where bk.User__id == strCurrentUserId
                      select bk;
            return PartialView("_Bookings", bks);
        }

        [OutputCache(Duration = 20, VaryByParam = "*")]
        public PartialViewResult Current()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var bookings = db.Bookings.Include(b => b.Ambulance).Include(b => b.User_);
            var bks = from bk in bookings
                      where bk.User__id == strCurrentUserId && bk.CurrentlyActive == true
                      select bk;
            return PartialView("_Bookings", bks);
        }

        public JsonResult GetBookings(string bookingFilter)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var bookings = db.Bookings.Include(b => b.Ambulance).Include(b => b.User_);
            var bks = from bk in bookings
                      where bk.User__id == strCurrentUserId 
                      select bk;
            if (bookingFilter == "yes")
            {

                var x = bks.Where(y => y.CurrentlyActive == false).ToList();

                return Json(x, JsonRequestBehavior.AllowGet);
            }
            return Json(bks.ToList(), JsonRequestBehavior.AllowGet);
        }

        /*
        [Authorize(Roles ="User")]
        public ActionResult Book(string user_id, int ambId) { 
            Ambulance amb = db.Ambulances.Find(ambId);
            var _amb = new Ambulance {
                id = ambId,
                aType_id = amb.aType_id,
                Driver_id = amb.Driver_id,
                Color = amb.Color,
                Plate = amb.Plate,
                Station_id = amb.Station_id,
                Available = false
            };
            Booking bk = new Booking
            {
                User__id = user_id,
                Ambulance_id = ambId
            };

            return RedirectToAction("Index");
        }
        */

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        
        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            booking.CurrentlyActive = false;
            int amb_id = booking.Ambulance_id;
            Ambulance amb = db.Ambulances.Find(amb_id);
            amb.Available = true;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idBooking,Ambulance_id,User__id,Date")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Ambulance_id = new SelectList(db.Ambulances, "id", "Station_id", booking.Ambulance_id);
            ViewBag.User__id = new SelectList(db.User_, "id", "id", booking.User__id);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
