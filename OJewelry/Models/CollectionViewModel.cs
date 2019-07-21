using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using OJewelry.Models;

namespace OJewelry.Models
{
    //CollectionListByCompany
    public class StyleModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        [Display(Name = "Style No.")]
        public String Num { get; set; }
        [Display(Name = "Memo'd")]
        public double Memod { get; set; }
        [Display(Name="Inventory")]
        public double Qty { get; set; }
        [Display(Name = "Retail Price")]
        public decimal RetialPrice { get; set; }
        public string Desc { get; set; }
        public decimal Cost { get; set; }
        public string Image { get; set; }
    }

    public class CollectionModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        [Required]
        public String Name { get; set; }
        public String Notes { get; set; }
        public List<StyleModel> Styles { get; set;}
    }

    public class CollectionViewModel
    {
        [Key] public int CompanyId { get; set; }
        public String CompanyName { get; set; }
        public List<CollectionModel> Collections { get; set; }
        public StyleModel style { get; }
        public Collection collection { get; }
    }
    // CollectionController Models
    public class CollectionsViewModel
    {
        public int CompanyId { get; set; }
        public String CompanyName { get; set; }
        public List<Collection> Collections { get; set; }
    }

    public class CollectionsDetailsModel
    {
        public int CompanyId { get; set; }
        public String CompanyName { get; set; }
        public Collection Collection { get; set; }
        public List<Style> Styles { get; set; }
    }

    public partial class Collection
    {
        public Collection(CollectionModel cm)
        {
            CompanyId = cm.CompanyId;
            Name = cm.Name;
            Notes = cm.Notes;
        }
    }
}

