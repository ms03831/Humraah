namespace Humraaah.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        [Key]
        public int idBooking { get; set; }

        public int Ambulance_id { get; set; }

        [Required]
        [StringLength(250)]
        public string User__id { get; set; }

        public DateTime Date { get; set; }
        public bool CurrentlyActive { get; set; }
        public virtual Ambulance Ambulance { get; set; }

        public virtual User_ User_ { get; set; }
    }
}
