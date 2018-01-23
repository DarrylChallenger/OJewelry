using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

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
        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

    }

    [MetadataType(typeof(ClientMetaData))]   // Client
    public partial class Client
    {
    }
    public partial class ClientMetaData
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
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
    }
    
    [MetadataType(typeof(ComponentMetaData))]   //component
    public partial class Component
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

        [Display(Name = "Labor")]
        public Nullable<decimal> MetalLabor { get; set; }

        [Display(Name = "CtWt")]
        public Nullable<int> StonesCtWt { get; set; }

        [Display(Name = "Size")]
        public string StoneSize { get; set; }

        [Display(Name = "$/Piece")]
        public Nullable<decimal> StonePPC { get; set; }

        [Display(Name = "Metal")]
        public string FindingsMetal { get; set; }

        [Display(Name = "Vendors")]
        public int VendorId { get; set; }

    }

    [MetadataType(typeof(ComponentTypeMetaData))]   // Component Type
    public partial class ComponentType
    {
    }
    public partial class ComponentTypeMetaData
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [Display(Name = "Sequence")]
        public int Sequence;
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
        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }

    [MetadataType(typeof(PresentersMetaData))]   
    public partial class Presenters
    {
    }
    public partial class PresentersMetaData
    {

        [Display(Name = "Location")]
        public String Name { get; }
    }

    [MetadataType(typeof(StyleMetaData))]   // Style
    public partial class Style
    {
    }
    public partial class StyleMetaData
    {

        [Display(Name = "Style No.")]
        [Required(ErrorMessage = "Style Number is required.")]
        public string StyleNum { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required.")]
        public string StyleName { get; set; }

        [Display(Name = "Desc")]
        public string Desc { get; set; }

        [Display(Name = "Metal Weight")]
        public Nullable<decimal> MetalWeight { get; set; }

        [Display(Name = "Intro Date")]
        public Nullable<System.DateTime> IntroDate { get; set; }

        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [Display(Name = "Width")]
        public Nullable<decimal> Width { get; set; }

        [Display(Name = "Length")]
        public Nullable<decimal> Length { get; set; }

        [Display(Name = "Chain Length")]
        public Nullable<decimal> ChainLength { get; set; }

        [Display(Name = "Retail Ratio")]
        public Nullable<decimal> RetailRatio { get; set; }

        [Display(Name = "Red Line Ratio")]
        public Nullable<decimal> RedlineRatio { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required.")]
        public int Quantity { get; set; }

        [Display(Name = "Jewelry Types")]
        public Nullable<int> JewelryTypeId { get; set; }

        [Display(Name = "Collections")]
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

        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [DisplayName("Vendor Phone")]
        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name="Vendor Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
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


}