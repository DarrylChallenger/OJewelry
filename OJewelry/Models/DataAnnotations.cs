using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace OJewelry.Models
{
    [MetadataType(typeof(CollectionMetaData))]
    public partial class Collection
    {
    }
    public partial class CollectionMetaData
    {
        [Display(Name ="Collection Name")]
        String Name { get; set; }
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

        [Display(Name="Vendor Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }


}