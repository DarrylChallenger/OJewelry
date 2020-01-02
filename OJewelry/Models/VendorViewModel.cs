using OJewelry.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class VendorViewModel
    {
        public VendorViewModel() { }
        public VendorViewModel(Vendor v)
        {
            Id = v.Id;
            CompanyId = v.CompanyId;
            Name = v.Name;
            Phone = v.Phone;
            Email = v.Email;
            Notes = v.Notes;
            SellsFindings = v.Type?.bFinding == vendorTypeEnum.Finding;
            SellsStones = v.Type?.bStone == vendorTypeEnum.Stone;
        }
        // Members
        public int Id { get; set; }
        public int? CompanyId { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Vendor Name is required.")]
        public string Name { get; set; }

        [PhoneOrEmail]
        [Display (Name="Vendor Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Vendor Phone number")]
        [StringLength(50)]
        public string Phone { get; set; }

        [PhoneOrEmail]
        [Display(Name = "Vendor Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "Notes")]
        [StringLength(50)]
        public string Notes { get; set; }

        private Vendor Vendor { get; set; }
        public String CompanyName { get; set; }

        [Display(Name = "Castings")]
        public bool SellsCastings { get; set; }

        [Display(Name = "Labor")]
        public bool SellsLabor { get; set; }

        [Display(Name = "Findings")]
        public bool SellsFindings { get; set; }

        [Display(Name = "Stones")]
        public bool SellsStones { get; set; }
        public vendorTypeEnum VendorType { get; set; }
    }

    public partial class Vendor
    {
        public Vendor(VendorViewModel vvm)
        {
            Id = vvm.Id;
            CompanyId = vvm.CompanyId;
            Name = vvm.Name;
            Phone = vvm.Phone;
            Email = vvm.Email;
            Notes = vvm.Notes;
            Type = new VendorType();
            Type.bFinding = vvm.SellsFindings ? vendorTypeEnum.Finding : 0;
            Type.bStone = vvm.SellsStones ? vendorTypeEnum.Stone : 0;
            Type.bCasting = vvm.SellsCastings ? vendorTypeEnum.Casting : 0;
            Type.bFinding = vvm.SellsLabor ? vendorTypeEnum.Labor : 0;
        }
    }
}