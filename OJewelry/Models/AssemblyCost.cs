using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class CostData
    {
        public CostData()
        {
            metalMarketPrice = new Dictionary<string, decimal>();
            metalMultiplier = new Dictionary<string, float>();
            finishingCosts = new Dictionary<string, decimal>();
            packagingCosts = new Dictionary<string, decimal>();
            settingsCosts = new Dictionary<string, decimal>();
        }
        public int companyId { get; set; }
        public string GetJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
        public DbSet<MetalCode> mc { set; get; }
        public Dictionary<string, decimal> metalMarketPrice { get; set; } // one for each metal type
        public Dictionary<string, float> metalMultiplier { get; set; } // one for each metal type
        public Dictionary<string, decimal> finishingCosts { get; set; } // one for each Jewelry Type
        public Dictionary<string, decimal> settingsCosts { get; set; } // one for each size
        public Dictionary<string, decimal> packagingCosts { get; set; } // one for each Jewelry Type
    }

    [Table("Cost")]
    public partial class AssemblyCost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int companyId { get; set; }
        public string costDataJSON { get; set; }
        public CostData GetCostDataFromJSON()
        {
            return JsonConvert.DeserializeObject<CostData>(costDataJSON);
        }

    }

    // Handle binding - 
    //http://www.hanselman.com/blog/ASPNETWireFormatForModelBindingToArraysListsCollectionsDictionaries.aspx
    //https://stackoverflow.com/questions/1300642/asp-mvc-net-how-to-bind-keyvaluepair?noredirect=1&lq=1, 
    //https://stackoverflow.com/questions/23435219/binding-mvc-view-to-ienumerablekeyvaluepairstring-bool-issues
}