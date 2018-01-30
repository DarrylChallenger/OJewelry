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
    public class ComponentsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Components
        public ActionResult Index(int CompanyId)
        {
            if (CompanyId == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var components = db.Components.Where(x => x.CompanyId == CompanyId).Include(c => c.Company).Include(c => c.ComponentType).Include(c => c.Vendor);
            ViewBag.Id = CompanyId;
            ViewBag.CompanyName = db.Companies.Find(CompanyId).Name;

            return View(components.ToList());
        }

        // GET: Components/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // GET: Components/Create
        public ActionResult Create(int CompanyId)
        {
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name");
            ViewBag.ComponentTypeId = new SelectList(db.ComponentTypes, "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name");
            ViewBag.Id = CompanyId;
            ViewBag.CompanyName = db.Companies.Find(CompanyId).Name;
            Component comp = new Component();
            comp.Price = 0;
            comp.PricePerHour = 0;
            comp.PricePerPiece = 0;
            comp.StonePPC = 0;
            return View(comp);
        }

        // POST: Components/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,ComponentTypeId,Name,VendorId,Quantity,Price,PricePerHour,PricePerPiece,MetalMetal,MetalLabor,StonesCtWt,StoneSize,StonePPC,FindingsMetal")] Component component)
        {
            if (ModelState.IsValid)
            {
                db.Components.Add(component);
                db.SaveChanges();
                return RedirectToAction("Index", new { component.CompanyId});
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", component.CompanyId);
            ViewBag.ComponentTypeId = new SelectList(db.ComponentTypes, "Id", "Name", component.ComponentTypeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", component.VendorId);
            return View(component);
        }

        // GET: Components/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", component.CompanyId);
            ViewBag.ComponentTypeId = new SelectList(db.ComponentTypes, "Id", "Name", component.ComponentTypeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", component.VendorId);
            Company co = db.Companies.Find(component.CompanyId);
            ViewBag.CompanyName = co.Name;
            return View(component);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,ComponentTypeId,Name,VendorId,Quantity,Price,PricePerHour,PricePerPiece,MetalMetal,MetalLabor,StonesCtWt,StoneSize,StonePPC,FindingsMetal")] Component component)
        {
            if (ModelState.IsValid)
            {
                db.Entry(component).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { component.CompanyId });
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", component.CompanyId);
            ViewBag.ComponentTypeId = new SelectList(db.ComponentTypes, "Id", "Name", component.ComponentTypeId);
            ViewBag.VendorId = new SelectList(db.Vendors, "Id", "Name", component.VendorId);
            return View(component);
        }

        // GET: Components/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Component component = db.Components.Find(id);
            if (component == null)
            {
                return HttpNotFound();
            }
            return View(component);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Component component = db.Components.Find(id);
            int companyId = component.CompanyId.Value;
            db.Components.Remove(component);
            db.SaveChanges();
            return RedirectToAction("Index", new { CompanyId = companyId });
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
