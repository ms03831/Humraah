using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Humraah.Models
{
    public class Address
    {
       
            #region Properties  
            public int Id { get; set; }
            [Required(ErrorMessage = "Locality is required")]
            public string Locality { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }

    }
}