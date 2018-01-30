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
    public class PresentersController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Presenters
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company co = db.Companies.Find(companyId);
            if (co == null)
            {
                return HttpNotFound();
            }
            var presenters = db.Presenters.Where(x=>x.CompanyId == companyId).Include(p => p.Company);
            ViewBag.CompanyName = co.Name;
            ViewBag.CompanyId = co.Id;
            return View(presenters.ToList());
        }

        // GET: Presenters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presenter presenter = db.Presenters.Find(id);
            presenter.Company = db.Companies.Find(presenter.CompanyId);
            if (presenter == null)
            {
                return HttpNotFound();
            }
            return View(presenter);
        }

        // GET: Presenters/Create
        public ActionResult Create(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company co = db.Companies.Find(companyId);
            if (co == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyName = co.Name;
            ViewBag.CompanyId = co.Id;
            return View();
        }

        // POST: Presenters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Email,CompanyId")] Presenter presenter)
        {
            if (ModelState.IsValid)
            {
                db.Presenters.Add(presenter);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = presenter.CompanyId });
            }

            presenter.Company = db.Companies.Find(presenter.CompanyId);
            return View(presenter);
        }

        // GET: Presenters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presenter presenter = db.Presenters.Find(id);
            if (presenter == null)
            {
                return HttpNotFound();
            }
            presenter.Company = db.Companies.Find(presenter.CompanyId);

            return View(presenter);
        }

        // POST: Presenters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Email,CompanyId")] Presenter presenter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(presenter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = presenter.CompanyId });
            }
            presenter.Company = db.Companies.Find(presenter.CompanyId);
            return View(presenter);
        }

        // GET: Presenters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presenter presenter = db.Presenters.Find(id);
            presenter.Company = db.Companies.Find(presenter.CompanyId);
            if (presenter == null)
            {
                return HttpNotFound();
            }
            return View(presenter);
        }

        // POST: Presenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Presenter presenter = db.Presenters.Find(id);
            int coid = presenter.CompanyId.Value;
            db.Presenters.Remove(presenter);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId = coid });
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
