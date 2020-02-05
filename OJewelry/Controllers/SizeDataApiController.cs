using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    public class SizeDataApiController : ApiController
    {

        private OJewelryDB db = new OJewelryDB();

        private class LocalSize
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        // GET api/<controller>/5
        public string Get(int companyId, string stone, string shape)
        {
            // GET api/<controller>/5

            // find all the sizes that exist for this stone, size & company
            List<LocalSize> stones = db.Stones.Include("Vendor").Include("Shape")
                .Where(st => st.Name == stone && st.Shape.Name == shape && st.CompanyId == companyId)
                .Select(st => new LocalSize() { Id = st.StoneSize, Name = st.StoneSize})
                .ToList();
            // make json string of results
            stones.Insert(0, new LocalSize() { Id="", Name = "Please Select a Size" });
            SelectList list = new SelectList(stones, "Id", "Name");

            string json = JsonConvert.SerializeObject(list);
            Trace.TraceInformation($"SizeDataApiController({companyId},{stone},{shape}): returns {json}");
            return json;
        }
    }
}