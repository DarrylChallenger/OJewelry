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
    public class JewelryTypesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: JewelryTypes
        public ActionResult Index()
        {
            var jewelryTypes = db.JewelryTypes;
            return View(jewelryTypes.ToList());
        }

        // GET: JewelryTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                return HttpNotFound();
            }
            return View(jewelryType);
        }

        // GET: JewelryTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JewelryTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PackagingCost,FinishingCost")] JewelryType jewelryType)
        {
            if (ModelState.IsValid)
            {
                db.JewelryTypes.Add(jewelryType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jewelryType);
        }

        // GET: JewelryTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                return HttpNotFound();
            }
            return View(jewelryType);
        }

        // POST: JewelryTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PackagingCost,FinishingCost")] JewelryType jewelryType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jewelryType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jewelryType);
        }

        // GET: JewelryTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                return HttpNotFound();
            }
            return View(jewelryType);
        }

        // POST: JewelryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            // if type is in use return message
            if (db.Styles.Where(s => s.JewelryTypeId == id).Count() != 0)
            {
                ModelState.AddModelError("JewelryType", jewelryType.Name + " is in use by at least one style.");
                return View(jewelryType);
            }
            db.JewelryTypes.Remove(jewelryType);
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
