using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    [DataContract]
    public class JewelryTypeJSON
    {
        public JewelryTypeJSON(JewelryType jewelryType, CostData cd)
        {
            id = jewelryType.Id;
            bUseLaborTable = jewelryType.bUseLaborTable;
            costData = cd;
        }
        [DataMember] int id { get; set; }
        [DataMember] bool bUseLaborTable { get; set; }
        [DataMember] CostData costData { get; set; }
    }
    
public class JewelryTypesApiController : ApiController
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: api/JewelryTypesApi/5
        [ResponseType(typeof(JewelryType))]
        public string GetJewelryType(int id)
        {
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            CostData cd = new CostData();
            cd.companyId = jewelryType.CompanyId.Value;
            // two dictionaries, one for each market & multiplier
            Dictionary<string, decimal> metalMarketPrice = new Dictionary<string, decimal>();  // one for each metal type
            Dictionary<string, float> metalMultiplier = new Dictionary<string, float>();  // one for each metal type

            // assign market and multiplier for each metalcode
            // Metal Costs
            foreach (MetalCode m in db.MetalCodes.Where(x => x.CompanyId == jewelryType.CompanyId)) // add company ID!!!
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
            if (!jewelryType.bUseLaborTable)
            {
                // Finishing and Packaging Costs
                foreach (JewelryType j in db.JewelryTypes.Where(x => x.CompanyId == jewelryType.CompanyId))
                {
                    // Finishing Costs: per Jewelry Type
                    if (cd.finishingCosts.Where(k => k.Key == j.Name).Count() == 0)
                    {
                        cd.finishingCosts.Add(j.Name, j.FinishingCost);
                    }
                    // Packaging Costs: per Jewelry Type
                    if (cd.packagingCosts.Where(k => k.Key == j.Name).Count() == 0)
                    {
                        cd.packagingCosts.Add(j.Name, j.PackagingCost);
                    }
                }
            }

            JewelryTypeJSON jt = new JewelryTypeJSON(jewelryType, cd);
            string json = JsonConvert.SerializeObject(jt);
            return json;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool JewelryTypeExists(int id)
        {
            return db.JewelryTypes.Count(e => e.Id == id) > 0;
        }
    }
}