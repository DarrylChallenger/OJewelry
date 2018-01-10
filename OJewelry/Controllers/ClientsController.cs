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
    public class ClientsController : Controller
    {
        private OJewelryDBEntities dc = new OJewelryDBEntities();

        // GET: Clients
        public ActionResult Index()
        {
            var clients = dc.Clients.Include(c => c.Company);
            return View(clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index","Home");
            }
            Client client = dc.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.CompanyID = new SelectList(dc.Companies, "Id", "Name");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Email,CompanyID")] Client client)
        {
            if (ModelState.IsValid)
            {
                dc.Clients.Add(client);
                dc.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CompanyID = new SelectList(dc.Companies, "Id", "Name", client.CompanyID);
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Client client = dc.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(dc.Companies, "Id", "Name", client.CompanyID);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Email,CompanyID")] Client client)
        {
            if (ModelState.IsValid)
            {
                dc.Entry(client).State = EntityState.Modified;
                dc.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyID = new SelectList(dc.Companies, "Id", "Name", client.CompanyID);
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Client client = dc.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = dc.Clients.Find(id);
            dc.Clients.Remove(client);
            dc.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dc.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
