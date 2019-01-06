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
    [Authorize]
    public class CollectionsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();
        // GET: Collections
        public ActionResult Index(int? CompanyId)
        {
            if (CompanyId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Company co = db.FindCompany(CompanyId);
            //co = db.Companies.Include("Collections").Include("Collections.Styles").Include("Collections.Styles.Memos").Where(c => c.Id == CompanyId).SingleOrDefault();
            if (co == null)
            {
                return HttpNotFound();
            }

            CollectionViewModel m = new CollectionViewModel();
            m.CompanyId = co.Id;
            m.CompanyName = co.Name;
            m.Collections = new List<CollectionModel>();
            foreach (Collection coll in co.Collections.OrderBy(c=>c.Name))
            {
                CollectionModel collM = new CollectionModel()
                {
                    Id = coll.Id,
                    CompanyId = coll.CompanyId,
                    Name = coll.Name
                };
                collM.Styles = new List<StyleModel>();
                foreach (Style sty in coll.Styles.OrderBy(s=>s.StyleName).ThenBy(s=>s.Desc))
                {
                    StyleModel styM = new StyleModel()
                    {
                        Id = sty.Id,
                        Image = sty.Image,
                        Desc = sty.Desc,
                        Name = sty.StyleName,
                        Num = sty.StyleNum,
                        Memod = sty.Memos.Sum(s => s.Quantity),
                        Qty = sty.Quantity,
                        RetialPrice = sty.RetailPrice ?? 0,
                        // Cost is the sum of the component prices
                        //Retail Price is the cost * retail ratio
                    };
                    collM.Styles.Add(styM);
                }
                m.Collections.Add(collM);
            }
            return View(m);

        }

        // GET: Collections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            Company co = db.FindCompany(collection.CompanyId);

            CollectionsDetailsModel cm = new CollectionsDetailsModel()
            {
                CompanyId = collection.CompanyId,
                CompanyName = co.Name,
                Collection = collection,
                Styles = collection.Styles.ToList(),
            };
            return View(cm);
        }

        // GET: Collections/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            CollectionModel cm = new CollectionModel()
            {
                CompanyId = id.Value,
            };
            ViewBag.CompanyName = db.FindCompany(id).Name;
            return View(cm);
        }

        // POST: Collections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionModel cm)
        {
            Collection collection = new Collection(cm);
            if (ModelState.IsValid)
            {
                db.Collections.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index", new { CompanyId = collection.CompanyId });
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", collection.CompanyId);
            return View(cm);
        }

        // GET: Collections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", collection.CompanyId);
            return View(collection);
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,Name,Notes")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { CompanyId = collection.CompanyId });
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", collection.CompanyId);
            return View(collection);
        }

        // GET: Collections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Collection collection = db.Collections.Find(id);
            if (db.Styles.Where(s => s.CollectionId == id).Count() != 0)
            {
                ModelState.AddModelError("Collection", collection.Name + " contains at least one style.");
                return View(collection);
            }
            db.Collections.Remove(collection);
            db.SaveChanges();
            return RedirectToAction("Index", new { CompanyId = collection.CompanyId });
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
