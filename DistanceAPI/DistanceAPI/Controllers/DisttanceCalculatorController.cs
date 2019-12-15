using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DistanceAPI.Models;

namespace DistanceAPI.Controllers
{
    public class DistanceCalculatorController : ApiController
    {
        [HttpGet]
        public decimal CalculateDistance(Address address1, Address address2)
        {
            decimal lat1 = address1.Lat;
            decimal lat2 = address2.Lat;
            decimal lon1 = address1.Lng;
            decimal lon2 = address2.Lng;


            if ((lat1 == lat2) && (lon1 == lon2)) {
                return 0;
                }
            else {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
                dist = dist* 60 * 1.1515;
            if (unit == 'K') {
                dist = dist* 1.609344;
            } else if (unit == 'N') {
                dist = dist* 0.8684;
            }
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
