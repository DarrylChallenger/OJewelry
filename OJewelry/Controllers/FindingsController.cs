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
    public class FindingsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Findings
        public ActionResult Index()
        {
            var findings = db.Findings.Include(f => f.Company).Include(f => f.Metal).Include(f => f.Vendor);
            return View(findings.ToList());
        }

        // GET: Findings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finding finding = db.Findings.Find(id);
            if (finding == null)
            {
                return HttpNotFound();
            }
            return View(finding);
        }

        // GET: Findings/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db._Companies, "Id", "Name");
            ViewBag.MetalCodeId = new SelectList(db.MetalCodes, "Id", "Code");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name");
            return View();
        }

        // POST: Findings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,Price,PricePerHour,PricePerPiece,MetalCodeId,Qty,FindingsMetal")] Finding finding)
        {
            if (ModelState.IsValid)
            {
                db.Findings.Add(finding);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db._Companies, "Id", "Name", finding.CompanyId);
            ViewBag.MetalCodeId = new SelectList(db.MetalCodes, "Id", "Code", finding.MetalCodeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", finding.VendorId);
            return View(finding);
        }

        // GET: Findings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finding finding = db.Findings.Find(id);
            if (finding == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db._Companies, "Id", "Name", finding.CompanyId);
            ViewBag.MetalCodeId = new SelectList(db.MetalCodes, "Id", "Code", finding.MetalCodeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", finding.VendorId);
            return View(finding);
        }

        // POST: Findings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,Price,PricePerHour,PricePerPiece,MetalCodeId,Qty,FindingsMetal")] Finding finding)
        {
            if (ModelState.IsValid)
            {
                db.Entry(finding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db._Companies, "Id", "Name", finding.CompanyId);
            ViewBag.MetalCodeId = new SelectList(db.MetalCodes, "Id", "Code", finding.MetalCodeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", finding.VendorId);
            return View(finding);
        }

        // GET: Findings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finding finding = db.Findings.Find(id);
            if (finding == null)
            {
                return HttpNotFound();
            }
            return View(finding);
        }

        // POST: Findings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Finding finding = db.Findings.Find(id);
            db.Findings.Remove(finding);
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
