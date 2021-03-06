namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class JewelryType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JewelryType()
        {
        }

        public int Id { get; set; }

        public int? CompanyId { get; set; }

        [Required(ErrorMessage ="Jewelry Type Name is required")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name ="Use Labor Table")]
        public bool bUseLaborTable { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal PackagingCost { get; set; }

        [Required]
        public decimal FinishingCost { get; set; }

        public virtual Company Company { get; set; }
    }
}
