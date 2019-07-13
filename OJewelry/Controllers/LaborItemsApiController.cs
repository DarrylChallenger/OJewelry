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
    public class LaborItemApiType
    {
        public LaborItemApiType(LaborItem li)
        {
            id = li.Id;
            pph = li.pph;
            ppp = li.ppp;
            Vendor = li.Vendor.Name;
        }

        [DataMember] int id { get; set; }
        [DataMember] decimal? pph { get; set; }
        [DataMember] decimal? ppp { get; set; }
        [DataMember] string Vendor { get; set; }
    }

    public class LaborItemsApiController : ApiController
    {

        private OJewelryDB db = new OJewelryDB();

        // GET: api/LaborItems/5
        [ResponseType(typeof(LaborItem))]
        public string GetLaborItem(int id)
        {
            LaborItem laborItem = db.LaborTable.Find(id);
            if (laborItem == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            laborItem.Vendor = db.Vendors.Find(laborItem.VendorId);
            LaborItemApiType lit = new LaborItemApiType(laborItem);
            string json = JsonConvert.SerializeObject(lit);
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

        private bool LaborItemExists(int id)
        {
            return db.LaborTable.Count(e => e.Id == id) > 0;
        }
    }
}