using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MetalCodesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: MetalCodes
        public ActionResult Index(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View(db.MetalCodes.Where(mc => mc.CompanyId == companyId).OrderBy(mc=>mc.Code).ToList());
        }

        // GET: MetalCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetalCode metalCode = db.MetalCodes.Find(id);
            if (metalCode == null)
            {
                return HttpNotFound();
            }
            return View(metalCode);
        }

        // GET: MetalCodes/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View();
        }

        // POST: MetalCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Desc,Market,Multiplier,CompanyId")] MetalCode metalCode)
        {
            if (ModelState.IsValid)
            {
                db.MetalCodes.Add(metalCode);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = metalCode.CompanyId });
            }
            ViewBag.CompanyId = metalCode.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(metalCode.CompanyId)?.Name;

            return View(metalCode);
        }

        // GET: MetalCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetalCode metalCode = db.MetalCodes.Find(id);
            if (metalCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = metalCode.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(metalCode.CompanyId)?.Name;
            return View(metalCode);
        }

        // POST: MetalCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Desc,Market,Multiplier,CompanyId")] MetalCode metalCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metalCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { metalCode.CompanyId });
            }
            ViewBag.CompanyId = metalCode.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(metalCode.CompanyId)?.Name;
            return View(metalCode);
        }

        // GET: MetalCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetalCode metalCode = db.MetalCodes.Find(id);
            if (metalCode == null)
            {
                return HttpNotFound();
            }
            return View(metalCode);
        }

        // POST: MetalCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MetalCode metalCode = db.MetalCodes.Find(id);
            if (db.Castings.Where(s => s.MetalCodeID == id).Count() != 0)
            {
                ModelState.AddModelError("MetalCode", metalCode.Desc + " is in use by at least one casting.");
                return View(metalCode);
            }
            int companyId = metalCode.CompanyId.Value;
            db.MetalCodes.Remove(metalCode);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId });
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
