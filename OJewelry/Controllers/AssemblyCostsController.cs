using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    public class AssemblyCostsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: AssemblyCosts/Edit/5
        public ActionResult Edit(int? companyId)
        {
            Company company;
            if (companyId !=null)
            {
                company = db.FindCompany(companyId);
            } else
            {
                return RedirectToAction("Index", "Companies");
            }

            if (companyId == null || company == null)
            {
                return RedirectToAction("Index", "Companies");
            }
            AssemblyCost assemblyCost = db.AssemblyCosts.FirstOrDefault(ac => ac.companyId == companyId);
            if (assemblyCost == null)
            {
                assemblyCost = new AssemblyCost
                {
                    companyId = companyId.Value,
                    costDataJSON = ""
                };
                db.AssemblyCosts.Add(assemblyCost);
                db.SaveChanges();
            }
            CostData cd = assemblyCost.GetCostDataFromJSON();
            if (cd == null)
            {
                cd = new CostData
                {
                    companyId = companyId.Value
                };
            }
            // Ensure that the CostData structure is fully expanded
            // Metal Costs
            int i = 0;
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
                    cd.settingsCosts.Add(st.StoneSize, 1);
                }
            }
            ViewBag.CompanyName = company.Name;
            return View(cd);
        }

        // POST: AssemblyCosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CostData costData)
        {
            if (ModelState.IsValid)
            {
                AssemblyCost assemblyCost = db.AssemblyCosts.Find(costData.companyId);
                assemblyCost.costDataJSON = costData.GetJSON();
                db.Entry(assemblyCost).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "");
            }
            return View(costData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
