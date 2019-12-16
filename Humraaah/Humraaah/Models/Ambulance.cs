namespace Humraaah.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ambulance")]
    public partial class Ambulance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ambulance()
        {
            Bookings = new HashSet<Booking>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(250)]
        public string Station_id { get; set; }

        public int Driver_id { get; set; }

        public int aType_id { get; set; }

        [Required]
        [StringLength(250)]
        public string Plate { get; set; }

        [Required]
        [StringLength(250)]
        public string Color { get; set; }

        public bool Available { get; set; }

        public virtual aType aType { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Station Station { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }
    }

    public class RegisterAmbulanceModel
    {

        [Required]
        [Display(Name = "Driver Name")]
        public string Driver { get; set; }

        [Required]
        [Range(01000000000, 11111111111)]
        [Display(Name = "Driver Phone")]
        public long DriverPhone { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Capacity")]
        public int Capacity { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Plate")]
        public string plate { get; set; }

        [Required]
        [Display(Name = "Color")]
        public string color { get; set; }

        [Required]
        [Display(Name = "Is Ambulance Available?")]
        public bool available { get; set; }

    }
}
