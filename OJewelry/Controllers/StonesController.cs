﻿using System;
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
        public ActionResult Index()
        {
            var stones = db.Stones.Include(s => s.Company).Include(s => s.Shape).Include(s => s.Vendor);
            return View(stones.ToList());
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
            return View(stone);
        }

        // GET: Stones/Create
        public ActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name");
            return View();
        }

        // POST: Stones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,CtWt,StoneSize,ShapeId,Price,Qty")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                db.Stones.Add(stone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", stone.CompanyId);
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", stone.VendorId);
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
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", stone.CompanyId);
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", stone.VendorId);
            return View(stone);
        }

        // POST: Stones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,CtWt,StoneSize,ShapeId,Price,Qty")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", stone.CompanyId);
            ViewBag.ShapeId = new SelectList(db.Shapes, "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", stone.VendorId);
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
            db.Stones.Remove(stone);
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
