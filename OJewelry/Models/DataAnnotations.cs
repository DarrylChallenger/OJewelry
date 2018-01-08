using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    [MetadataType(typeof(CollectionMetaData))]
    public partial class Collection
    {
    }

    public class CollectionMetaData
    {
        [Display(Name ="Collection Name")]
        String Name { get; set; }
    }
    [MetadataType(typeof(VendorMetaData))]
    public partial class Vendor
    {
    }

    public class VendorMetaData
    {
        [Required(ErrorMessage = "Name is required.")] String Name { get; set; }
        [Required(ErrorMessage = "Phone is required.")] String Phone { get; set; }
        [Required(ErrorMessage = "Email is required.")] String Email { get; set; }
    }


}