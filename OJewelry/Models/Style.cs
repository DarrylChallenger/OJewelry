namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Style
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Style()
        {
            Memos = new HashSet<Memo>();
            StyleCastings = new HashSet<StyleCasting>();
            StyleStones = new HashSet<StyleStone>();
            StyleFindings = new HashSet<StyleFinding>(); 
            StyleLabors = new HashSet<StyleLabor>();
            StyleMiscs = new HashSet<StyleMisc>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string StyleNum { get; set; }

        [Required]
        [StringLength(50)]
        public string StyleName { get; set; }

        [StringLength(150)]
        public string Desc { get; set; }

        public int? JewelryTypeId { get; set; }

        public int CollectionId { get; set; }

        public int? MetalWeight { get; set; }

        public int? MetalWtUnitId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IntroDate { get; set; }

        [StringLength(255)]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public decimal? Width { get; set; }

        public decimal? Length { get; set; }

        public decimal? ChainLength { get; set; }

        public decimal? RetailPrice { get; set; }

        public decimal? RetailRatio { get; set; }

        public decimal? RedlineRatio { get; set; }

        public double Quantity { get; set; }

        public double UnitsSold { get; set; }

        public virtual Collection Collection { get; set; }

        public virtual JewelryType JewelryType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Memo> Memos { get; set; }

        public virtual MetalWeightUnit MetalWeightUnit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleCasting> StyleCastings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleStone> StyleStones { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleFinding> StyleFindings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleLabor> StyleLabors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleMisc> StyleMiscs { get; set; }
    }
}
