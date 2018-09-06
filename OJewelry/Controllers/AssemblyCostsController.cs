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

        // GET: AssemblyCosts
        public ActionResult Index(int companyId)
        {
            return View(db.AssemblyCosts.ToList());
        }

        // GET: AssemblyCosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssemblyCost assemblyCost = db.AssemblyCosts.Find(id);
            if (assemblyCost == null)
            {
                return HttpNotFound();
            }
            return View(assemblyCost);
        }

        // GET: AssemblyCosts/Create
        public ActionResult Create(int companyId)
        {
            return View();
        }

        // POST: AssemblyCosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,costDataJSON")] AssemblyCost assemblyCost)
        {
            if (ModelState.IsValid)
            {
                db.AssemblyCosts.Add(assemblyCost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assemblyCost);
        }

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
            CostData cd = JsonConvert.DeserializeObject<CostData>(assemblyCost.costDataJSON);
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
                    cd.finishingCosts.Add(jt.Name, i++);
                }
                // Packaging Costs: per Jewelry Type
                if (cd.packagingCosts.Where(k => k.Key == jt.Name).Count() == 0)
                {
                    cd.packagingCosts.Add(jt.Name, i++);
                }
            }
            // Settings Costs 
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
                assemblyCost.costDataJSON = JsonConvert.SerializeObject(costData);
                db.Entry(assemblyCost).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index", "");
            }
            return View(costData);
        }

        // GET: AssemblyCosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssemblyCost assemblyCost = db.AssemblyCosts.Find(id);
            if (assemblyCost == null)
            {
                return HttpNotFound();
            }
            return View(assemblyCost);
        }

        // POST: AssemblyCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssemblyCost assemblyCost = db.AssemblyCosts.Find(id);
            db.AssemblyCosts.Remove(assemblyCost);
            db.SaveChanges();
            return RedirectToAction("Index");
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
