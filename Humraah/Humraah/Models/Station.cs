namespace Humraah.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Station
    {
        public int Id { get; set; }

        public int CountAmbulances { get; set; }

        [Required]
        public string Organization { get; set; }

        public int Address_Id { get; set; }
    }
}
