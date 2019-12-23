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
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Humraaah.Controllers
{
    [Authorize(Roles = "Station")]
    public class AmbulancesController : Controller
    {
        private HumraahContext db = new HumraahContext();

        /*
        public AmbulancesController()
        {
            this.ApplicationDbContext = new ApplicationDbContext();
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }
        
        // GET: Ambulances
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }
        */
        [OutputCache(Duration = 20, VaryByParam = "*")]
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var ambulancesTemp = db.Ambulances.Include(a => a.aType).Include(a => a.Driver).Include(a => a.Station);
            var ambulances = from ambulance in ambulancesTemp
                             where ambulance.Station_id.Equals(strCurrentUserId)
                             select ambulance;
            return View(ambulances.ToList());
        }

        // GET: Ambulances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;
            var ambulancesTemp = db.Ambulances.Include(a => a.aType).Include(a => a.Driver).Include(a => a.Station);
            var ambulance1 = (from ambulance in ambulancesTemp
                              where ambulance.Station_id.Equals(strCurrentUserId) && ambulance.id == id
                              select ambulance).FirstOrDefault();
            if (ambulance1 == null)
            {
                return HttpNotFound();
            }
            return View(ambulance1);
        }

        // GET: Ambulances/Create
        public ActionResult Create()
        {
            var types = from type in db.aTypes
                        select type.vehicleType;

            var TypeLst = new List<string>();
            TypeLst.AddRange(types.Distinct());

            ViewBag.Type = new SelectList(TypeLst);
            return View();

        }

        
        // POST: Ambulances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterAmbulanceModel ambulance)
        {
            if (ModelState.IsValid)
            {
                var dri = from dr in db.Drivers
                          where dr.Name == ambulance.Driver && dr.Phone == ambulance.DriverPhone
                          select dr;
                Driver driv;

                if (dri.ToList().Count == 0)
                {
                    driv = new Driver { Name = ambulance.Driver, Phone = ambulance.DriverPhone };
                    db.Drivers.Add(driv);
                    db.SaveChanges();
                }
                else
                {
                    driv = dri.FirstOrDefault();
                }

                var tp = from t in db.aTypes
                         where t.vehicleType == ambulance.Type && t.capacity == ambulance.Capacity
                         select t;
                aType type;

                if (tp.ToList().Count == 0)
                {
                    type = new aType { capacity = ambulance.Capacity, vehicleType = ambulance.Type };
                    db.aTypes.Add(type);
                    db.SaveChanges();
                }
                else
                {
                    type = tp.FirstOrDefault();
                }

                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                string strCurrentUserId = user.Id;

                Ambulance amb = new Ambulance
                {
                    aType_id = type.id,
                    Driver_id = driv.id,
                    Color = ambulance.color,
                    Plate = ambulance.plate,
                    Station_id = strCurrentUserId,
                    Available = ambulance.available
                };



                db.Ambulances.Add(amb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ambulance);
        }

        // GET: Ambulances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulance ambulance = db.Ambulances.Find(id);
            if (ambulance == null)
            {
                return HttpNotFound();
            }
            ViewBag.aType_id = new SelectList(db.aTypes, "id", "vehicleType", ambulance.aType_id);
            ViewBag.Driver_id = new SelectList(db.Drivers, "id", "id", ambulance.Driver_id);
            ViewBag.Station_id = new SelectList(db.Stations, "id", "Organization", ambulance.Station_id);
            return View(ambulance);
        }

        // POST: Ambulances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Station_id,Driver_id,aType_id,Plate,Color,Available")] Ambulance ambulance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ambulance).State = EntityState.Modified;
                db.SaveChanges();
                var bks = db.Bookings.Where(x => x.Ambulance == ambulance);
                foreach(var bk in bks)
                {
                    bk.CurrentlyActive = false;
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.aType_id = new SelectList(db.aTypes, "id", "vehicleType", ambulance.aType_id);
            ViewBag.Driver_id = new SelectList(db.Drivers, "id", "id", ambulance.Driver_id);
            ViewBag.Station_id = new SelectList(db.Stations, "id", "Organization", ambulance.Station_id);
            return View(ambulance);
        }

        // GET: Ambulances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ambulance ambulance = db.Ambulances.Find(id);
            if (ambulance == null)
            {
                return HttpNotFound();
            }
            return View(ambulance);
        }

        // POST: Ambulances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ambulance ambulance = db.Ambulances.Find(id);
            db.Ambulances.Remove(ambulance);
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
