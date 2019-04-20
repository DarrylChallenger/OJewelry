using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Text;

namespace OJewelry.Models
{
    [MetadataType(typeof(BuyerMetaData))]  // Buyer
    public partial class Buyer
    {
    }
    public partial class BuyerMetaData
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        String Name { get; set; }

        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

    }

    [MetadataType(typeof(CastingMetaData))]   // Casting
    public partial class Casting
    {
    }
    public partial class CastingMetaData
    {
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Labor")]
        [DataType(DataType.Currency)]
        public decimal Labor { get; set; }
    }

    [MetadataType(typeof(ClientMetaData))]   // Client
    public partial class Client
    {
    }
    public partial class ClientMetaData
    {
        [Display(Name = "Name")]
        //[Required(ErrorMessage = "Name is required.")]
        [RequiredIfNotRemoved]
        public String Name { get; set; }

        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

    }

    [MetadataType(typeof(CollectionMetaData))]   // collection
    public partial class Collection
    {
    }
    public partial class CollectionMetaData
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }
    }

    [MetadataType(typeof(CompanyMetaData))]   //company
    public partial class Company
    {
    }
    public partial class CompanyMetaData
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [Display(Name = "Addr 1")]
        public string StreetAddr { get; set; }

        [Display(Name = "Addr 2")]
        public string Addr2 { get; set; }

        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(150)]
        public String Email { get; set; }

        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

    }

    [MetadataType(typeof(ComponentMetaData))]   //component
    public partial class ComponentX
    {
    }
    public partial class ComponentMetaData
    {
        [Display(Name ="Component Types")]
        public int ComponentTypeId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required.")]
        //[RegularExpression(@"(\.\d{2}){1}$")]
        public decimal? Price { get; set; }

        [Display(Name = "$/Hour")]
        [Required(ErrorMessage = "Hourly cost is required.")]
        //[RegularExpression(@"(\.\d{2}){1}$")]
        public Nullable<decimal> PricePerHour { get; set; }

        [Display(Name = "$/Piece")]
        [Required(ErrorMessage = "Price per piece is required.")]
        //[RegularExpression(@"(\.\d{2}){1}$")]
        public decimal? PricePerPiece { get; set; }

        [Display(Name ="Metal")]
        public string MetalMetal { get; set; }

        [Display(Name = "Metal")]
        public string FindingsMetal { get; set; }

        [Display(Name = "Metal")]
        public int MetalCodeId { get; set; }

        [Display(Name = "Labor")]
        public Nullable<decimal> MetalLabor { get; set; }

        [Display(Name = "CtWt")]
        public Nullable<int> StonesCtWt { get; set; }

        [Display(Name = "Size")]
        public string StoneSize { get; set; }

        [Display(Name = "$/Piece")]
        public Nullable<decimal> StonePPC { get; set; }

        [Display(Name = "Vendors")]
        public int VendorId { get; set; }

    }

    [MetadataType(typeof(ContactMetaData))]   // Contact
    public partial class Contact
    {
    }
    public partial class ContactMetaData
    {

        [Display(Name = "Name")]
        //[Required(ErrorMessage = "Name is required.")]
        [RequiredIfNotRemoved]
        public String Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

    }

    [MetadataType(typeof(FindingMetaData))]   // Finding
    public partial class Finding
    {
    }
    public partial class FindingMetaData
    {
        [Required(ErrorMessage = "You must select a finding.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String Name { get; set; }
        
        [Display(Name = "WT(dwt)")]
        public decimal? Weight { get; set; }
    }

    [MetadataType(typeof(JewelryTypeMetaData))]   //Jewelry Type
    public partial class JewelryType
    {
    }
    public partial class JewelryTypeMetaData
    {
        [Display(Name ="Jewelry Types")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Packaging Cost")]
        [DataType(DataType.Currency)]
        public decimal PackagingCost { get; set; }

        [Required]
        [Display(Name = "Finishing Cost")]
        [DataType(DataType.Currency)]
        public decimal FinishingCost { get; set; }

    }

    [MetadataType(typeof(LedgerMetaData))]  // Ledger
    public partial class Ledger
    {
    }
    public partial class LedgerMetaData
    {
    }

    [MetadataType(typeof(MemoMetaData))]   // Memo
    public partial class Memo
    {
    }
    public partial class MemoMetaData
    {
    }

    [MetadataType(typeof(MetalCodeMetaData))]   // MetalCode
    public partial class MetalCode
    {
    }
    public partial class MetalCodeMetaData
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Desc { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Market { get; set; }

        [Required]
        public float Multiplier { get; set; }
    }


    [MetadataType(typeof(PresenterMetaData))]   // Presenter
    public partial class Presenter
    {
    }
    public partial class PresenterMetaData
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [Display(Name ="Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }

    
    [MetadataType(typeof(StoneMetaData))]   // Stone
    public partial class Stone
    {
    }
    public partial class StoneMetaData
    {
        [Required(ErrorMessage = "You must select a stone.")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Size")]
        public string StoneSize { get; set; }

        [Display(Name = "CT")]
        public int? CtWt { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Setting Cost")]
        public decimal SettingCost { get; set; }
    }


    [MetadataType(typeof(StyleMetaData))]   // Style
    public partial class Style
    {
    }
    public partial class StyleMetaData
    {

        [Display(Name = "STYLE #")]
        [Required(ErrorMessage = "Style Number is required.")]
        public string StyleNum { get; set; }

        [Display(Name = "STYLE NAME")]
        [Required(ErrorMessage = "Name is required.")]
        public string StyleName { get; set; }

        [Display(Name = "DESC")]
        public string Desc { get; set; }

        [Display(Name = "METAL WT")]
        public Nullable<decimal> MetalWeight { get; set; }

        [Display(Name ="NOTE")]
        public string MetalWtNote { get; set; }

        [Display(Name = "INTRO DATE")]
        [DataType(DataType.Date)]
        /*
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        */
        public Nullable<System.DateTime> IntroDate { get; set; }

        [Display(Name = "W")]
        public Nullable<decimal> Width { get; set; }

        [Display(Name = "L")]
        public Nullable<decimal> Length { get; set; }

        [Display(Name = "CHAIN LENGTH")]
        public Nullable<decimal> ChainLength { get; set; }

        [Display(Name = "Retail Ratio")]
        public Nullable<decimal> RetailRatio { get; set; }

        [Display(Name = "Red Line Ratio")]
        public Nullable<decimal> RedlineRatio { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Display(Name = "JEWELRY TYPE")]
        public Nullable<int> JewelryTypeId { get; set; }

        [Display(Name = "COLLECTION")]
        public int CollectionId { get; set; }
    }

    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {
    }

    public partial class VendorMetaData
    {
        [Display(Name ="Vendors")]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [DisplayName("Vendor Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name="Vendor Email")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        [Display(Name = "Notes")]
        [StringLength(50)]
        public string Notes { get; set; }
    }

    /*
    [MetadataType(typeof(XXXMetaData))]   // template
    public partial class XXX
    {
    }
    public partial class XXXMetaData
    {

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }
    }
         [Display(Name = "")]
         [Required(ErrorMessage = "is required.")]
  */

    partial class Style
    {
        public String RenderInfo()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("Rendered contents");
            return sb.ToString();
        }
    }
}