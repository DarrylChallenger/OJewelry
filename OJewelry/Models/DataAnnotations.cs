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


}