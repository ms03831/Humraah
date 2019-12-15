namespace Humraah.DAL
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
            User_ = new HashSet<User_>();
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
        public virtual ICollection<User_> User_ { get; set; }
    }
}
