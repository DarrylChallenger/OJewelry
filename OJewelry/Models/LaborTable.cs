using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using static OJewelry.Classes.Validations;
using OJewelry.Classes;

namespace OJewelry.Models
{
    [Table("LaborTable")]
    public partial class LaborItem
    {
        public LaborItem()
        {
            State = LMState.Dirty;
            ppp = null;
            pph = null;
            //Vendor = new Vendor();
            StyleLaborItems = new HashSet<StyleLaborTableItem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [RequiredIfNotRemoved]
        [StringLength(100)]
        [Display(Name ="Labor")]
        public string Name { get; set; }

        [HourlyXORStatic]
        [Display(Name = "$/PC")]
        [DataType(DataType.Currency)]
        public decimal? ppp { get; set; }

        [Display(Name = "$/HR")]
        [DataType(DataType.Currency)]
        public decimal? pph { get; set; }

        [RequiredIfNotRemoved]
        [Display(Name ="Vendor")]
        public int VendorId { get; set; }

        [NotMapped]
        public LMState State { get; set; }

        [NotMapped]
        public SelectList selectList { get; set; }

        public virtual Company Company { get; set; }
        public virtual Vendor Vendor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleLaborTableItem> StyleLaborItems { get; set; }

    }
}