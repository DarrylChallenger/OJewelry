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
        public JewelryTypeJSON(JewelryType jewelryType)
        {
            id = jewelryType.Id;
            bUseLaborTable = jewelryType.bUseLaborTable;
        }
        [DataMember] int id { get; set; }
        [DataMember] bool bUseLaborTable { get; set; }
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
            JewelryTypeJSON jt = new JewelryTypeJSON(jewelryType);
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