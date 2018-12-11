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
    public class ShapesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Shapes
        public ActionResult Index(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View(db.Shapes.Where(s => s.CompanyId == companyId).ToList());
        }

        // GET: Shapes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shape shape = db.Shapes.Find(id);
            if (shape == null)
            {
                return HttpNotFound();
            }
            return View(shape);
        }

        // GET: Shapes/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View();
        }

        // POST: Shapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,CompanyId")] Shape shape)
        {
            if (ModelState.IsValid)
            {
                db.Shapes.Add(shape);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = shape.CompanyId });
            }
            ViewBag.CompanyId = shape.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(shape.CompanyId)?.Name;

            return View(shape);
        }

        // GET: Shapes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shape shape = db.Shapes.Find(id);
            if (shape == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = shape.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(shape.CompanyId)?.Name;
            return View(shape);
        }

        // POST: Shapes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CompanyId")] Shape shape)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shape).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { shape.CompanyId });
            }
            ViewBag.CompanyId = shape.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(shape.CompanyId)?.Name;
            return View(shape);
        }

        // GET: Shapes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shape shape = db.Shapes.Find(id);
            if (shape == null)
            {
                return HttpNotFound();
            }
            return View(shape);
        }

        // POST: Shapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shape shape = db.Shapes.Find(id);
            if (db.Castings.Where(c => c.MetalCodeID == id).Count() !=0) {
                ModelState.AddModelError("Shapes", shape.Name + " is in use by at least one style.");
                return View(shape);

            }
            int companyId = shape.CompanyId.Value;
            db.Shapes.Remove(shape);
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
