using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public partial class AssemblyCost
    {
        public void Validate(OJewelryDB db, int _companyId)
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
            // Zero out and save the data
            // Ensure that the CostData structure is fully expanded
            // Metal Costs
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
            state = db.Entry(this).State;
            if (state == System.Data.Entity.EntityState.Detached)
            {
                db.AssemblyCosts.Add(this);
            }
            if (state != System.Data.Entity.EntityState.Unchanged)
            {
                db.SaveChanges();
            }

        }

    }
}