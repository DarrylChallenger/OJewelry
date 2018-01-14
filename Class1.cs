using System;

public class Class1
{
	public Class1()
	{
	}
}
using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace OJewelry.Models
{
    [MetadataType(typeof(XXXMetaData))]  //buyer
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   //client
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(CollectionMetaData))]   // collection
    public partial class Collection
    {
    }
    public partial class CollectionMetaData
    {
        [Display(Name = "Collection Name")]
        String Name { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   //company
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   //component
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   // Component Type
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   //Jewelry Type
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   // Memo
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   // Presenter
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]  // Ledger
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(XXXMetaData))]   // Style
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }

    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {
    }

    public partial class VendorMetaData
    {
        [Required(ErrorMessage = "Name is required.")]
        public String Name { get; set; }

        [DisplayName("Vendor Phone")]
        [Required(ErrorMessage = "Phone is required.")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public String Phone { get; set; }

        [Display(Name = "Vendor Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }

    /*
    [MetadataType(typeof(XXXMetaData))]   // template
    public partial class XXX
    {
    }
    public partial class XXX
    {
        [Display(Name = "Name")]
        String Prop { get; set; }
    }
    */


}