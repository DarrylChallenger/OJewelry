using DocumentFormat.OpenXml.Drawing.Charts;

namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Findings")]
    public partial class Finding
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Finding()
        {
            StyleFinding = new HashSet<StyleFinding>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //roll db Back; include auto increments

        public int? CompanyId { get; set; }
        public int? VendorId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Desc { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999999999999999", ErrorMessage = "Price cannot be $0") ]
        public decimal Price { get; set; }

        public decimal? Weight { get; set; }

        public int Qty { get; set; } // Inventory

        [StringLength(2048)]
        public string Note { get; set; }

        public virtual Company Company { get; set; }

        public virtual Vendor Vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleFinding> StyleFinding { get; set; }
    }
}
