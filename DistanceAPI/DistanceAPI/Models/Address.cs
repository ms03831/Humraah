using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistanceAPI.Models
{
    public class Address
    {
        public int id { get; set; }

        [StringLength(250)]
        public string Locality { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }
    }


}