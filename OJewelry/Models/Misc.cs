namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Misc")]
    public partial class Misc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Misc()
        {
            StyleMiscs = new HashSet<StyleMisc>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Vendor { get; set; }

        [StringLength(50)]
        public string Desc { get; set; }

        public decimal? PricePerPiece { get; set; }

        public int? Qty { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleMisc> StyleMiscs { get; set; }
    }
}
