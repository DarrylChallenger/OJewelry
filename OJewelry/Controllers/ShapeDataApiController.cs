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
    public class ShapeDataApiController : ApiController
    {
        private class LocalShape
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        private OJewelryDB db = new OJewelryDB();
        // GET api/<controller>/5
        public string Get(int companyId, string stone)
        {
            // find all the shapes that exist for this stone & company
            List<LocalShape> shapes = db.Stones.Where(st => st.Name == stone && st.CompanyId == companyId)
                .Join(db.Shapes, st => st.ShapeId, sh => sh.Id, (st, sh) => new LocalShape()
                {
                    Id = sh.Id.ToString(),
                    Name = sh.Name
                }).Distinct().ToList();
            // make json string of results
            shapes.Insert(0, new LocalShape() { Id = "", Name = "Please Select a Shape" });
            SelectList list = new SelectList(shapes, "Id", "Name");
            
            string json = JsonConvert.SerializeObject(list);
            Trace.TraceInformation($"ShapeDataApiController({companyId},{stone}): returns {json}");

            return json;
        }
    }
}