using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DistanceAPI.Models;
using System.Web.Http;

namespace DistanceAPI.Controllers
{
    public class DistanceCalculatorController : ApiController
    {
        [HttpGet]
        public double CalculateDistance(decimal lat1_, decimal lat2_, decimal lon1_, decimal lon2_)
        {
            double lat1 = (double)lat1_;
            double lat2 = (double)lat2_;
            double lon1 = (double)lon1_;
            double lon2 = (double)lon2_;


            if ((lat1 == lat2) && (lon1 == lon2)) {
                return 0;
                }
            else {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist* 60 * 1.1515;
                dist = dist* 1.609344;
                return (dist);
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }
    }
}
