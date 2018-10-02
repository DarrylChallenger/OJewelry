namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Casting
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Casting()
        {
            StyleCastings = new HashSet<StyleCasting>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? VendorId { get; set; }

        public int? MetalCodeID { get; set; }

        public decimal? Price { get; set; }

        public decimal? Labor { get; set; }

        public int? Qty { get; set; }

        [Required]
        [Range(0.0,99999.0,ErrorMessage ="Weight must be positive")]
        public decimal MetalWeight { get; set; }

        [Required]
        public int MetalWtUnitId { get; set; }

        public virtual Vendor Vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleCasting> StyleCastings { get; set; }
    }
}
