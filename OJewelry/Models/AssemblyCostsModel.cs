using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

    public partial class AssemblyCost
    {
        /*
        public void Load(OJewelryDB db, int _companyId)
        {
            System.Data.Entity.EntityState state;
            state = db.Entry(this).State;
            companyId = _companyId;
            CostData cd;

            if (costDataJSON == null || costDataJSON == "")
            {
                cd = new CostData
                {
                    companyId = _companyId,
                };
            } else {
                cd = GetCostDataFromJSON();
            }
            // Ensure that the CostData structure is fully expanded
            if (cd.metalMarketPrice == null) { cd.metalMarketPrice = new Dictionary<string, decimal>(); }
            if (cd.metalMultiplier == null) { cd.metalMultiplier = new Dictionary<string, float>(); }
            if (cd.finishingCosts == null) { cd.finishingCosts = new Dictionary<string, decimal>(); }
            if (cd.packagingCosts == null) { cd.packagingCosts = new Dictionary<string, decimal>(); }
            if (cd.settingsCosts == null) { cd.settingsCosts = new Dictionary<string, decimal>(); }

            // Metal Costs
            foreach (MetalCode m in db.MetalCodes)
            {
                // Metal Market Price
                if (cd.metalMarketPrice.Where(k => k.Key == m.Code).Count() == 0)
                {
                    cd.metalMarketPrice.Add(m.Code, 1);
                }
                // Metal Multiplier
                if (cd.metalMultiplier.Where(k => k.Key == m.Code).Count() == 0)
                {
                    cd.metalMultiplier.Add(m.Code, 1);
                }
            }
            // Finishing Costs
            foreach (JewelryType jt in db.JewelryTypes)
            {
                // Finishing Costs: per Jewelry Type
                if (cd.finishingCosts.Where(k => k.Key == jt.Name).Count() == 0)
                {
                    cd.finishingCosts.Add(jt.Name, 0);
                }
                // Packaging Costs: per Jewelry Type
                if (cd.packagingCosts.Where(k => k.Key == jt.Name).Count() == 0)
                {
                    cd.packagingCosts.Add(jt.Name, 0);
                }
            }
            // Settings Costs 
            foreach (Stone st in db.Stones)
            {
                if (cd.settingsCosts.Where(k => k.Key == st.StoneSize).Count() == 0)
                {
                    cd.settingsCosts.Add(st.StoneSize, 0);
                }
            }
            costDataJSON = cd.GetJSON();
            db.Entry(this);
        }

        public CostData GetCostDataFromJSON()
        {
            return JsonConvert.DeserializeObject<CostData>(costDataJSON);
        }
        */
    }
}