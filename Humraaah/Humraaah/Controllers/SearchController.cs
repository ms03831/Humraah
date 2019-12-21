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
    public class SearchController : Controller
    {
        private HumraahContext db = new HumraahContext();

        // GET: Search
        /*
        public ActionResult Index()
        {
            var ambulances = from amb in db.Ambulances
                             orderby amb.Station_id
                             select amb;

            var Locality = new List<string>();

            var loc = from a in db.Addresses
                      orderby a.Locality
                      select a.Locality;

            Locality.AddRange(loc.Distinct());
            ViewBag.locality = new SelectList(Locality);

            var Org = new List<string>();

            var org_ = from stat in db.Stations
                       orderby stat.Organization
                       select stat.Organization;


            Org.AddRange(org_.Distinct());
            ViewBag.Organization = new SelectList(Org);

            var dist = new List<string>();
            dist.Add("Yes");
            dist.Add("No");

            ViewBag.Distance = new SelectList(dist);

            return View(ambulances.ToList());
        }
        */
//        [OutputCache(Duration = 60, VaryByParam ="*")]
        public ActionResult Index(string locality, string Distance, string Organization)
        {
            var ambulances = from amb in db.Ambulances
                             where amb.Available == true
                             select amb;
            if (Response.Cookies["Search"]["loc"] == null)
            {
                Response.Cookies["Search"]["loc"] = locality;
            }

            if (Response.Cookies["Search"]["dist"] == null)
            {
                Response.Cookies["Search"]["dist"] = Distance;
            }

            if (Response.Cookies["Search"]["org"] == null)
            {
                Response.Cookies["Search"]["org"] = Organization;
            }

            var Locality = new List<string>();

            var loc = from a in db.Addresses
                          orderby a.Locality
                          select a.Locality;

            Locality.AddRange(loc.Distinct());
            ViewBag.locality = new SelectList(Locality);

            var Org = new List<string>();

            var org_ = from stat in db.Stations
                       orderby stat.Organization
                       select stat.Organization;


            Org.AddRange(org_.Distinct());
            ViewBag.Organization = new SelectList(Org);

            var dist = new List<string>();
            dist.Add("Yes");
            dist.Add("No");

            ViewBag.Distance = new SelectList(dist);
            
            if (User.Identity.IsAuthenticated)
            {
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                string strCurrentUserId = user.Id;
                User_ s = db.User_.Find(strCurrentUserId);
                if (s != null)
                    ViewBag.Role = 1;
            }

            if (!String.IsNullOrEmpty(locality))
            {
                ambulances = ambulances.Where(s => s.Station.Address.Locality.Contains(locality));
            }

            if (!String.IsNullOrEmpty(Organization))
            {
                ambulances = ambulances.Where(s => s.Station.Organization.Contains(Organization));
            }

            if (!String.IsNullOrEmpty(Distance))
            {
                if (Distance == "Yes")
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    string strCurrentUserId = user.Id;
                    User_ s = db.User_.Find(strCurrentUserId);
                    Address adr = s.Address;
                    //int lat1, lat2, lon1, lon2;
                    //lat1 = adr.Lat;
                    //lon1 = adr.Lng;
                    Dictionary<Ambulance, double> myDict = new Dictionary<Ambulance, double>(); 
                    foreach(var amb in ambulances)
                    {
                        //lat2 = amb.Station.Address.Lat;
                        //lon2 = amb.Station.Address.Lng;
                        double dist_ = DistanceCalculator.DistanceCalculate(adr, amb.Station.Address);
                        myDict.Add(amb, dist_);
                    }

                    var ax = (from entry in myDict
                                  orderby entry.Value ascending
                                  select entry.Key);

                    return View(ax.ToList());
                }
            }

            return View(ambulances.ToList());

        }


        public ActionResult SearchByLocalityName(string locality)
        {
            var ambulances = from amb in db.Ambulances
                              where amb.Available == true && amb.Station.Address.Locality.Contains(locality)
                              select amb;
            return View(ambulances.ToList());
        }

        public JsonResult GetSearchingData(string locality, string Distance, string Organization)
        {
            var ambulances = from amb in db.Ambulances
                             where amb.Available == true
                             select amb;

            if (!String.IsNullOrEmpty(locality))
            {
                ambulances = ambulances.Where(s => s.Station.Address.Locality.Contains(locality));
            }

            if (!String.IsNullOrEmpty(Organization))
            {
                ambulances = ambulances.Where(s => s.Station.Organization.Contains(Organization));
            }

            if (!String.IsNullOrEmpty(Distance))
            {
                if (Distance == "Yes")
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    string strCurrentUserId = user.Id;
                    User_ s = db.User_.Find(strCurrentUserId);
                    Address adr = s.Address;
                    //int lat1, lat2, lon1, lon2;
                    //lat1 = adr.Lat;
                    //lon1 = adr.Lng;
                    Dictionary<Ambulance, double> myDict = new Dictionary<Ambulance, double>();
                    foreach (var amb in ambulances)
                    {
                        //lat2 = amb.Station.Address.Lat;
                        //lon2 = amb.Station.Address.Lng;
                        double dist_ = DistanceCalculator.DistanceCalculate(adr, amb.Station.Address);
                        myDict.Add(amb, dist_);
                    }

                    var ax = (from entry in myDict
                              orderby entry.Value ascending
                              select entry.Key);

                    return Json(ax.ToList(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(ambulances.ToList(), JsonRequestBehavior.AllowGet);
        }



        [Authorize(Roles ="User")]

        // GET: Search/AmbulanceDetails/5
        public ActionResult AmbulanceDetails(int? id)
        {
            if (id != null)
            {
                Ambulance ambulance = db.Ambulances.Find(id);
                if (ambulance == null)
                {
                    return HttpNotFound();
                }
                return View(ambulance);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [Authorize(Roles = "User")]
        public ActionResult Book(int id)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            string strCurrentUserId = user.Id;

            if (id != null)
            {
                Ambulance amb = db.Ambulances.Find(id);
                if (amb == null)
                {
                    return HttpNotFound();
                }

                /*
                var _amb = new Ambulance
                {
                    id = id,
                    aType_id = amb.aType_id,
                    Driver_id = amb.Driver_id,
                    Color = amb.Color,
                    Plate = amb.Plate,
                    Station_id = amb.Station_id,
                    Available = false
                };
                */


                Booking bk = new Booking
                {
                    Date = DateTime.Now,
                    Ambulance_id = amb.id,
                    User__id = strCurrentUserId,
                    CurrentlyActive = true
                };
                amb.Available = false; db.SaveChanges();

                /*
                                db.Ambulances.Attach(_amb);
                                db.Entry(_amb).Property(X => X.Available).IsModified = true;
                */
                db.Bookings.Add(bk);
                db.SaveChanges();
                return RedirectToAction("Index", "Bookings");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
