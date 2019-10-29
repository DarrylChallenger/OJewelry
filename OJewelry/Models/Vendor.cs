namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using OJewelry.Classes;

    public partial class Vendor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vendor()
        {
            //Components = new HashSet<Component>();
        }

        public int Id { get; set; }

        public int? CompanyId { get; set; }

        //[Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Notes { get; set; }

        //public int TypeId { get; set; } // Don't use this one

        public VendorType Type { get; set;  }

        //public virtual VendorType Type { get;set; }

        public virtual Company Company { get; set; }

        /*
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Component> Components { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Component> Components { get; set; }*/

        public IEnumerable<VendorTypeEnumObj> GetEnumOjbs()
        {
            return Type.GetEnumOjbs();
        }
    }
}
