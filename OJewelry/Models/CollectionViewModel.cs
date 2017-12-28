using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class StyleModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        [Display(Name = "Style No.")]
        public String Num { get; set; }
        [Display(Name = "Memo'd")]
        public int Memod { get; set; }
        [Display(Name="Inventory")]
        public int Qty { get; set; }
        [Display(Name = "Retial Price")]
        public float RetialPrice { get; set; }
    }

    public class CollectionModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public String Name { get; set; }
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
}