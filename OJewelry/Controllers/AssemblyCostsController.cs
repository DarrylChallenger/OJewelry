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
    /*
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
            AssemblyCost assemblyCost = db.AssemblyCosts.Find(companyId);
            if (assemblyCost == null)
            {
                assemblyCost = new AssemblyCost();
                assemblyCost.companyId = companyId.Value;
            }
            assemblyCost.Load(db, companyId.Value);
            CostData cd = assemblyCost.GetCostDataFromJSON();

            ViewBag.CompanyName = company.Name;
            cd.mc = db.MetalCodes;
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
                AssemblyCostX assemblyCost = db.AssemblyCosts.Find(costData.companyId);
                if (assemblyCost == null)
                {
                    assemblyCost = new AssemblyCostX();
                    assemblyCost.companyId = costData.companyId;
                    assemblyCost.costDataJSON = costData.GetJSON();
                    db.AssemblyCosts.Add(assemblyCost);
                    db.SaveChanges();
                }
                else
                {
                    assemblyCost.costDataJSON = costData.GetJSON();
                    db.Entry(assemblyCost).State = EntityState.Modified;
                    db.SaveChanges();
                }
                //return RedirectToAction("Index", "");
            }
            Company company = db.FindCompany(costData.companyId);
            ViewBag.CompanyName = company.Name;
            costData.mc = db.MetalCodes;
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
    */
}
