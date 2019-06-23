using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Newtonsoft.Json;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    public class DropdownApiController : ApiController
    {
        private OJewelryDB db = new OJewelryDB();
        // GET api/<controller>/5
        public string Get(int companyId, string dropdown)
        {
            SelectList list = null;
            if (dropdown == "LaborTableVendors") {
                List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == companyId).ToList();
                vendors.Insert(0, new Vendor() {
                    Name = "Choose a Vendor"
                });
                list = new SelectList(vendors, "Id", "Name");
            }
            string json = JsonConvert.SerializeObject(list);
            return json;
        }
    }
}