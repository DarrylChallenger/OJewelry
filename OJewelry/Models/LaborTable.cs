using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using static OJewelry.Classes.Validations;

namespace OJewelry.Models
{
    [Table("LaborTable")]
    public class LaborItem
    {
        public LaborItem()
        {
            State = LMState.Dirty;
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
        public decimal? pph { get; set; }

        [Display(Name = "$/HR")]
        public decimal? ppp { get; set; }

        [RequiredIfNotRemoved]
        [Display(Name ="Vendor")]
        public int VendorId { get; set; }

        [NotMapped]
        public LMState State { get; set; }

        [NotMapped]
        public SelectList selectList { get; set; }

        public virtual Company Company { get; set; }
        public virtual Vendor Vendor { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HourlyXORStatic : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LaborItem item = (LaborItem)validationContext.ObjectInstance;
            var property = validationContext.ObjectType.GetProperty("State");
            if (property != null)
            {
                var state = property.GetValue(validationContext.ObjectInstance, null);
                if (state.ToString() == "Unadded" || state.ToString() == "Deleted")
                {
                    return ValidationResult.Success;
                }
            }

            if ((item.pph != null && item.ppp != null) || (item.pph == null && item.ppp == null))
            {
                return new ValidationResult("Please specify either $/hour or $/piece, but not both");
            }
            return ValidationResult.Success;
        }
    }


}