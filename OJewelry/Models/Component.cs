namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Component
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Component()
        {
            StyleComponents = new HashSet<StyleComponent>();
        }

        public int Id { get; set; }

        public int? CompanyId { get; set; }

        public int ComponentTypeId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? VendorId { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }

        [Column(TypeName = "money")]
        public decimal? PricePerHour { get; set; }

        [Column(TypeName = "money")]
        public decimal? PricePerPiece { get; set; }

        [StringLength(50)]
        public string MetalMetal { get; set; }

        [Column(TypeName = "money")]
        public decimal? MetalLabor { get; set; }

        public int? StonesCtWt { get; set; }

        [StringLength(50)]
        public string StoneSize { get; set; }

        [Column(TypeName = "money")]
        public decimal? StonePPC { get; set; }

        [StringLength(50)]
        public string FindingsMetal { get; set; }

        public virtual Company Company { get; set; }

        public virtual ComponentType ComponentType { get; set; }

        public virtual Vendor Vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleComponent> StyleComponents { get; set; }
    }
}
