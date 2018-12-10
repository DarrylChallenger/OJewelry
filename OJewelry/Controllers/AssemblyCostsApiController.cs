using Newtonsoft.Json;
using OJewelry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OJewelry.Controllers
{
    public class AssemblyCostsApiController : ApiController
    {
        private OJewelryDB db = new OJewelryDB();
        // GET api/<controller>/5
        public string Get(int companyId)
        {
            CostData cd = new CostData();
            // two dictionaries, one for each market & multiplier
            Dictionary<string, decimal> metalMarketPrice = new Dictionary<string, decimal>();  // one for each metal type
            Dictionary<string, float> metalMultiplier = new Dictionary<string, float>();  // one for each metal type

            // assign market and multiplier for each metalcode
            // Metal Costs
            foreach (MetalCode m in db.MetalCodes) // add company ID!!!
            {
                // Metal Market Price
                if (cd.metalMarketPrice.Where(k => k.Key == m.Code).Count() == 0)
                {
                    cd.metalMarketPrice.Add(m.Code, m.Market);
                }
                // Metal Multiplier
                if (cd.metalMultiplier.Where(k => k.Key == m.Code).Count() == 0)
                {
                    cd.metalMultiplier.Add(m.Code, m.Multiplier);
                }
            }
            // Finishing and Packaging Costs
            foreach (JewelryType jt in db.JewelryTypes) // add company ID!!!
            {
                // Finishing Costs: per Jewelry Type
                if (cd.finishingCosts.Where(k => k.Key == jt.Name).Count() == 0)
                {
                    cd.finishingCosts.Add(jt.Name, jt.FinishingCost);
                }
                // Packaging Costs: per Jewelry Type
                if (cd.packagingCosts.Where(k => k.Key == jt.Name).Count() == 0)
                {
                    cd.packagingCosts.Add(jt.Name, jt.PackagingCost);
                }
            }

            string costDataJSON = cd.GetJSON();

            return costDataJSON;
        }
    }
}
