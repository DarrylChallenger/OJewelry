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
    public class StylesController : Controller
    {
        private OJewelryDBEntities db = new OJewelryDBEntities();

        // GET: Styles
        public ActionResult Index(int CollectionId)
        {
            var styles = db.Styles.Include(s => s.Collection).Where(i => i.CollectionId == CollectionId).Include(s => s.JewelryType);
            ViewBag.CollectionName = db.Collections.Find(CollectionId).Name;
            ViewBag.CollectionId = CollectionId;
            ViewBag.CompanyId = db.Collections.Find(CollectionId).CompanyId;
            return View(styles.ToList());
        }

        // GET: Styles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return View(style);
        }

        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return View(style);
        }

        // GET: Styles/Create
        public ActionResult Create(int collectionId)
        {
            Collection co = db.Collections.Find(collectionId);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name");
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == co.CompanyId), "Id", "Name");
            Style s = new Style()
            {
                CollectionId = collectionId,
            };
            s.Collection = db.Collections.Find(collectionId);
            return View(s);
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StyleNum,StyleName,Desc,JewelryTypeId,CollectionId,IntroDate,Image,Width,Length,ChainLength,RetailRatio,RedlineRatio,Quantity")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Styles.Add(style);
                db.SaveChanges();
                return RedirectToAction("Index", new { CollectionID = style.CollectionId });
            }
            Collection co = db.Collections.Find(style.CollectionId);
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", style.JewelryTypeId);
            style.Collection = db.Collections.Find(style.Collection);
            return View(style);
        }

        // GET: Styles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            Collection co = db.Collections.Find(style.CollectionId);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", style.JewelryTypeId);
            return View(style);
        }

        // POST: Styles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StyleNum,StyleName,Desc,JewelryTypeId,CollectionId,IntroDate,Image,Width,Length,ChainLength,RetailRatio,RedlineRatio,Quantity")] Style style)
        {
            if (ModelState.IsValid)
            {
                db.Entry(style).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { CollectionID = style.CollectionId });
            }
            Collection co = db.Collections.Find(style.CollectionId);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", style.JewelryTypeId);
            return View(style);
        }

        // GET: Styles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            return View(style);
        }

        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Style style = db.Styles.Find(id);
            int collectionId = style.CollectionId;
            db.Styles.Remove(style);
            db.SaveChanges();
            return RedirectToAction("Index", new { CollectionID = collectionId });
        }

        public ActionResult Memo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Move a Style in or out of Memo
            OJewelryDBEntities dc = new OJewelryDBEntities();
            Style style = dc.Styles.Find(id);
            MemoViewModel m = new MemoViewModel();
            m.style = new StyleModel()
            {
                Id = style.Id,
                Name = style.StyleName,
                Num = style.StyleNum,
                Qty = style.Quantity,
                Memod = style.Memos.Sum(s => s.Quantity)
            };
            m.SendReturnMemoRadio = 1;
            m.NewExistingPresenterRadio = 1;

            m.Memos = new List<MemoModel>();
            m.numPresentersWithStyle = 0;
            foreach (Memo i in dc.Memos)
            {
                MemoModel mm = new MemoModel()
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    date = i.Date,
                    PresenterName = i.Presenter.Name,
                    PresenterPhone = i.Presenter.Phone,
                    PresenterEmail = i.Presenter.Email,
                    Notes = i.Notes
                };
                m.Memos.Add(mm);
                m.numPresentersWithStyle++;
            }
            m.Presenters = new List<SelectListItem>();
            m.CompanyId = style.Collection.CompanyId;
            foreach (Presenter i in dc.Presenters.Where(w => w.CompanyId == m.CompanyId))
            {
                SelectListItem sli = new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                };
                m.Presenters.Add(sli);
            }
            m.PresenterName = "initname";
            ViewBag.CollectionId = style.CollectionId;
            return View(m);
        }

        [HttpPost]
        public ActionResult Memo(MemoViewModel m)
        {
            ModelState.Clear();
            OJewelryDBEntities dc = new OJewelryDBEntities();
            // populate style data
            Style sdb = dc.Styles.Find(m.style.Id);
            m.style.Name = sdb.StyleName;
            m.style.Num = sdb.StyleNum;
            m.style.Qty = sdb.Quantity;

            if (m.SendReturnMemoRadio == 1)
            {
                if (m.NewExistingPresenterRadio == 2)
                {
                    //Memo a new presenter
                    if (String.IsNullOrEmpty(m.PresenterName))
                    {
                        ModelState.AddModelError("Presenter Name", "Name is required for new Presenters.");
                    }
                    if (String.IsNullOrEmpty(m.PresenterEmail) && String.IsNullOrEmpty(m.PresenterPhone))
                    {
                        ModelState.AddModelError("Presenter Contact Info", "Phone or Email is required for new Presenters.");
                    }
                    Presenter p = new Presenter()
                    {
                        CompanyId = m.CompanyId,
                        Name = m.PresenterName,
                        Email = m.PresenterEmail,
                        Phone = m.PresenterPhone,
                    };
                    dc.Presenters.Add(p);
                    m.PresenterId = p.Id;
                }
                else
                {
                    // Memo an existing presenter - nothing to validate in this case as they just selected a Presenter from the list
                }
                // create new memo, reduce inventory
                sdb.Quantity -= m.SendQty;
                String note = "Sending " + m.SendQty.ToString() + " items to " + m.PresenterName + " on " + DateTime.Now.ToString();
                Memo mo = new Memo()
                {
                    Quantity = m.SendQty,
                    Date = DateTime.Now,
                    Notes = note,
                    PresenterID = m.PresenterId,
                    StyleID = m.style.Id,
                };
                dc.Memos.Add(mo);
                if (m.SendQty > m.style.Qty)
                {
                    ModelState.AddModelError("Send Quantity", "You cannot memo more items than you have in inventory.");
                }
                if (m.SendQty < 1)
                {
                    ModelState.AddModelError("Send Quantity", "You can only memo a positive number of items.");
                }
            }
            else
            {
                //Return Items from Presenter
                // iterate thru the memos to take items back. Increase the inventory as appropriate. If all items are returned, delete the memo
                foreach (MemoModel memo in m.Memos)
                {
                    if (memo.RetrunQty < 0)
                    {
                        ModelState.AddModelError("Return Style", "You can only return a positive number to inventory.");
                    }
                    if (memo.RetrunQty > 0)
                    {
                        if (memo.RetrunQty > memo.Quantity)
                        {
                            ModelState.AddModelError("Return Style", "You can't return more items than were memo'd out.");
                        }
                        // update db
                        Memo mdb = dc.Memos.Find(memo.Id);
                        if (mdb.Quantity == memo.RetrunQty)
                        {
                            // remove the row and remove item from collection
                            dc.Memos.Remove(mdb);
                        }
                        else
                        {
                            // decrease the amount
                            mdb.Quantity -= memo.RetrunQty;
                        }
                        sdb.Quantity += memo.RetrunQty;
                    }
                    // ReturnQty is 0, no action
                }

            }
            if (ModelState.IsValid)
            {
                // Save changes, go to clientlist
                dc.SaveChanges();
                //return ClientList();
                //return View(m);
            }
            return Memo(m.style.Id);

            // Re-present the page to allow for corrections
            GetPresenters(dc, m, m.CompanyId);

            int numPresentersWithStyle = m.numPresentersWithStyle;
            m.numPresentersWithStyle = 0;
            Dictionary<int, int> d = new Dictionary<int, int>();
            for (int ii = 0; ii < numPresentersWithStyle; ii++)
            {
                d.Add(m.Memos[ii].Id, ii);
            }

            foreach (Memo memo in dc.Memos)
            {
                if (d.ContainsKey(memo.Id))
                {
                    memo.Presenter = dc.Presenters.Find(memo.PresenterID);
                    m.Memos[d[memo.Id]].Quantity = memo.Quantity;
                    m.Memos[d[memo.Id]].date = memo.Date;
                    m.Memos[d[memo.Id]].PresenterName = memo.Presenter.Name;
                    m.Memos[d[memo.Id]].PresenterPhone = memo.Presenter.Phone;
                    m.Memos[d[memo.Id]].PresenterEmail = memo.Presenter.Email;
                    m.Memos[d[memo.Id]].Notes = memo.Notes;
                    m.style.Memod += dc.Memos.Find(memo.Id).Quantity;
                }
                else
                {
                    MemoModel mm = new MemoModel()
                    {
                        Id = memo.Id,
                        Quantity = memo.Quantity,
                        date = memo.Date,
                        PresenterName = memo.Presenter.Name,
                        PresenterPhone = memo.Presenter.Phone,
                        PresenterEmail = memo.Presenter.Email,
                        Notes = memo.Notes
                    };
                    m.style.Memod += dc.Memos.Find(memo.Id).Quantity;

                    m.Memos.Add(mm);
                }
                m.numPresentersWithStyle++;
            }
            return View(m.style.Id);
        }

        public ActionResult Sell(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Move a Style in or out of Memo
            OJewelryDBEntities dc = new OJewelryDBEntities();
            Style style = dc.Styles.Find(id);
            MemoViewModel m = new MemoViewModel();
            m.style = new StyleModel()
            {
                Id = style.Id,
                Name = style.StyleName,
                Num = style.StyleNum,
                Qty = style.Quantity,
                Memod = style.Memos.Sum(s => s.Quantity)
            };
            return View(m);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        void GetPresenters(OJewelryDBEntities dc, MemoViewModel m, int CompanyId)
        {
            m.Presenters = new List<SelectListItem>();
            foreach (Presenter i in dc.Presenters.Where(w => w.CompanyId == CompanyId))
            {
                SelectListItem sli = new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                };
                m.Presenters.Add(sli);
            }
        }

    }
}
