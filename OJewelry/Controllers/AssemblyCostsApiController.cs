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
            AssemblyCost assemblyCost = db.AssemblyCosts.Find(companyId);
            if (assemblyCost == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return assemblyCost.costDataJSON;
        }
    }
}