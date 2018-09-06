using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using OJewelry.Models;

namespace OJewelry
{
    public class StoneMatchingController : ApiController
    {
        private OJewelryDB db = new OJewelryDB();

        // GET api/<controller>/5
        public string Get(int companyId, string stone, string shape, string size)
        {
            string result;
            // find item in Stones with stoneId, shapeId, and size. If found, return it. Otherwise return not found.
            Stone stn = db.Stones.Include("Vendor").Include("Shape")
                .FirstOrDefault(st => st.Name == stone && st.Shape.Name == shape && st.StoneSize == size && st.CompanyId == companyId );
            if (stn == null)
            {
                string empty = "";
                result = JsonConvert.SerializeObject(new { CtWt = empty, VendorName = empty, Price = 0 });
            }
            else
            {
                // make json string of results
                string CtWt = stn.CtWt.GetValueOrDefault().ToString();
                string VendorName = stn.Vendor == null ? "" : stn.Vendor.Name ?? "";
                result = JsonConvert.SerializeObject(new { CtWt = CtWt, VendorName = VendorName, Price = stn.Price});
            }
            return result;
        }
    }
}