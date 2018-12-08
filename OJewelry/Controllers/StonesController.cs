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
    public class StonesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Stones
        public ActionResult Index(int companyId)
        {
            var stones = db.Stones.Where(st => st.CompanyId == companyId).Include(s => s.Company).Include(s => s.Shape).Include(s => s.Vendor);
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return View(stones.OrderBy(f => f.Company.Name).ThenBy(g => g.Name).ThenBy(h=>h.Shape.Name).ThenBy(i=>i.StoneSize).ToList());
        }

        // GET: Stones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stone stone = db.Stones.Find(id);
            if (stone == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;
            return View(stone);
        }
        
        // GET: Stones/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name");
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            Stone stone = new Stone
            {
                CompanyId = companyId
            };
            return View(stone);
        }

        // POST: Stones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,VendorId,Name,CtWt,StoneSize,ShapeId,Price,SettingCost")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                db.Stones.Add(stone);
                db.SaveChanges();
                return RedirectToAction("Index", new {companyId = stone.CompanyId});
            }

            ViewBag.CompanyId = stone.CompanyId;
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", stone.VendorId);
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;
            return View(stone);
        }

        // GET: Stones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stone stone = db.Stones.Find(id);
            if (stone == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", stone.VendorId);
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;
            return View(stone);
        }

        // POST: Stones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,CtWt,StoneSize,ShapeId,Price,Qty,SettingCost")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = stone.CompanyId });
            }
            ViewBag.CompanyId = stone.CompanyId;
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", stone.VendorId);
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;

            return View(stone);
        }

        // GET: Stones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stone stone = db.Stones.Find(id);
            if (stone == null)
            {
                return HttpNotFound();
            }
            return View(stone);
        }

        // POST: Stones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stone stone = db.Stones.Find(id);
            int companyId = stone.CompanyId.Value;
            db.Stones.Remove(stone);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId = companyId});
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
