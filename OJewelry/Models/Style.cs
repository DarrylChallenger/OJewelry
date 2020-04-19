namespace OJewelry.Models
{
    using OJewelry.Classes;
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
            StyleLaborItems = new HashSet<StyleLaborTableItem>();
            StyleMiscs = new HashSet<StyleMisc>();
            StyleNum = "blank";
        }

        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string StyleNum { get; set; }

        [Required]
        [StringLength(50)]
        public string StyleName { get; set; }

        [StringLength(150)]
        public string Desc { get; set; }

        [GreaterThanZero]
        public int? JewelryTypeId { get; set; }

        public int CollectionId { get; set; }

        public decimal? MetalWeight { get; set; }

        public int? MetalWtUnitId { get; set; }

        [StringLength(50)]
        public string MetalWtNote { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IntroDate { get; set; }

        [StringLength(255)]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        public string Width { get; set; }

        public string Length { get; set; }

        public string ChainLength { get; set; }

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
        public virtual ICollection<StyleLaborTableItem> StyleLaborItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleMisc> StyleMiscs { get; set; }
    }
}
