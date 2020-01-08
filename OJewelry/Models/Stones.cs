namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stones")]
    public partial class Stone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stone()
        {
            StyleStone = new HashSet<StyleStone>();
            /*Vendor= new Vendor();
            Company= new Company();
            Shape= new Shape();*/
            Price = 0;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CompanyId { get; set; }
        public int? VendorId { get; set; }

        [StringLength(50)]
        public string Name { get; set; } // Refers to Stone Type

        [StringLength(50)]
        [Display(Name ="Name")]
        public string Label { get; set; } // Refers to name used by company

        [StringLength(50)]
        public string Desc { get; set; }

        public int? CtWt { get; set; }

        [StringLength(50)]
        public string StoneSize { get; set; }

        public int? ShapeId { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999999999999999", ErrorMessage = "Price cannot be $0")]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Range(typeof(decimal), "0.01", "9999999999999999999999999999", ErrorMessage = "Setting Cost cannot be $0")]
        [Column(TypeName = "money")]
        public decimal SettingCost { get; set; }

        public int Qty { get; set; } // Inventory

        [StringLength(2048)]
        [Display(Name = "Notes")]
        public string Note { get; set; }

        [StringLength(1024)]
        [Display(Name = "Parent Handle")]
        public string ParentHandle { get; set; }

        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(1024)]
        public string Tags { get; set; }

        public virtual Company Company { get; set; }

        public virtual Vendor Vendor { get; set; }

        public virtual Shape Shape { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleStone> StyleStone { get; set; }
    }
}
