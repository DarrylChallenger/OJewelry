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
    public class StylesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Styles
        public ActionResult Index(int CollectionId)
        {
            var styles = db.Styles.Include(s => s.Collection).Where(i => i.CollectionId == CollectionId).OrderBy(s=>s.StyleNum).Include(s => s.JewelryType);
            ViewBag.CollectionName = db.Collections.Find(CollectionId).Name;
            ViewBag.CollectionId = CollectionId;
            ViewBag.CompanyId = db.Collections.Find(CollectionId).CompanyId;
            return View(styles.ToList());
        }

        // GET: Styles/Details/5
        public ActionResult Details(int? id)
        {
            StyleViewModel svm = new StyleViewModel();
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            svm.Style = db.Styles.Find(id);
            if (svm.Style == null)
            {
                return HttpNotFound();
            }
            svm.Populate(id, db);
            return View(svm);
        }

        public ActionResult Print(int? id)
        {
            StyleViewModel sm = new StyleViewModel();
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            sm.Style = db.Styles.Find(id);
            if (sm.Style == null)
            {
                return HttpNotFound();
            }
            sm.Populate(id, db);
            return View(sm);
        }

        // GET: Styles/Create
        public ActionResult Create(int collectionId)
        {
            Collection co = db.Collections.Find(collectionId);
            StyleViewModel svm = new StyleViewModel();
            svm.CompanyId = co.CompanyId;
            svm.SVMOp = SVMOperation.Create;
            svm.Populate(null, db);
            svm.Style.Collection = db.Collections.Find(collectionId);
            svm.Style.CollectionId = collectionId;
            svm.CompanyId = svm.Style.Collection.CompanyId;

            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name");
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit");
            //ViewBag.JewelryTypes = new SelectList(db.JewelryTypes);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name");
            return View(svm);
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,StyleNum,StyleName,Desc,JewelryTypeId,CollectionId,IntroDate,Image,Width,Length,ChainLength,RetailRatio,RedlineRatio,Quantity")] Style style)
        public ActionResult Create(StyleViewModel svm)
        {
            svm.SVMOp = SVMOperation.Create;
            int i = db.Styles.
                Join(db.Collections, s => s.CollectionId, col => col.Id, (s, c) => new
                {
                    StyleId = s.Id,
                    StyleNum = s.StyleNum,
                    CompanyId = c.CompanyId,
                }).Where(x => x.CompanyId == svm.CompanyId  && x.StyleNum == svm.Style.StyleNum).Count();
            if (i != 0) // is there a style with the same number for this company?
            {
                ModelState.AddModelError("", "Style with this name already exists for "
                    + db.FindCompany(svm.CompanyId).Name + ".");
            }
            else {
                svm.SVMState = SVMStateEnum.Added;
            }
            return Edit(svm);
        }

        // GET: Styles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            StyleViewModel svm = new StyleViewModel();
            svm.Style = db.Styles.Find(id);
            if (svm.Style == null)
            {
                return HttpNotFound();
            }
            Collection co = db.Collections.Find(svm.Style.CollectionId);
            svm.CompanyId = co.CompanyId;
            svm.Populate(id, db);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", svm.Style.MetalWtUnitId);
            return View(svm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StyleViewModel svm)
        {
            //ModelState.Clear();
            // Save the Style and all edited components; add the new ones and remove the deleted ones
            if (db.Entry(svm.Style).State != EntityState.Added) db.Entry(svm.Style).State = EntityState.Modified;
            // Iterate thru the components
            // Castings
            if (svm.Castings != null)
            {

                foreach (CastingComponent c in svm.Castings)
                {
                    Casting casting;
                    StyleCasting sc;
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            casting = new Casting(c);
                            // add a new link
                            db.Castings.Add(casting);
                            sc = new StyleCasting()
                            {
                                CastingId = casting.Id,
                                StyleId = svm.Style.Id
                            };
                            db.StyleCastings.Add(sc);
                            break;
                        case SVMStateEnum.Deleted:
                            sc = db.StyleCastings.Where(x => x.StyleId == svm.Style.Id && x.CastingId == c.Id).SingleOrDefault();
                            casting = db.Castings.Find(c.Id);
                            db.StyleCastings.Remove(sc);
                            db.Castings.Remove(casting);
                            break;
                        case SVMStateEnum.Dirty:
                            casting = db.Castings.Find(c.Id);
                            casting.Set(c);
                            /*
                            // Update the Syle-Casting Link
                            sc = db.StyleCastings.Where(x => x.StyleId == svm.Style.Id && x.CastingId == c.Id).SingleOrDefault();
                            */
                            break;
                        case SVMStateEnum.Clean: // No updates
                        default:
                            break;
                    }
                }
            }

            // Stones
            if (svm.Stones != null)
            {
                foreach (StoneComponent c in svm.Stones)
                {
                    StyleComponent sc;

                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            sc = new StyleComponent() { StyleId = svm.Style.Id, ComponentId = c.Id };
                            sc.ComponentId = c.Id;
                            sc.Quantity = c.Qty;
                            db.StyleComponents.Add(sc);
                            break;
                        case SVMStateEnum.Deleted:
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.Id == c.scId).SingleOrDefault();
                            db.StyleComponents.Remove(sc);
                            break;
                        case SVMStateEnum.Dirty:
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.Id == c.scId).SingleOrDefault();
                            sc.ComponentId = c.Id;
                            sc.Quantity = c.Qty;
                            break;
                        case SVMStateEnum.Clean:
                        default:
                            break;
                    }
                }
            }

            // Findings
            if (svm.Findings != null)
            {
                foreach (FindingsComponent c in svm.Findings)
                {
                    StyleComponent sc;
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            /*
                            component = new Component(c);
                            db.Components.Add(component);
                            */
                            sc = new StyleComponent() { StyleId = svm.Style.Id, ComponentId = c.Id };
                            sc.ComponentId = c.Id;
                            sc.Quantity = c.Qty;
                            db.StyleComponents.Add(sc);
                            break;
                        case SVMStateEnum.Deleted:
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.Id == c.scId).SingleOrDefault();
                            db.StyleComponents.Remove(sc);
                            break;
                        case SVMStateEnum.Dirty:
                            /*
                            component = db.Components.Find(c.Id);
                            component.Set(c);
                            */
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.Id == c.scId).SingleOrDefault();
                            sc.ComponentId = c.Id;
                            sc.Quantity = c.Qty;
                            break;
                        case SVMStateEnum.Clean: // No updates
                        default:
                            break;
                    }
                }
            }

            // Labors
            if (svm.Labors != null)
            {
                foreach (LaborComponent c in svm.Labors)
                {
                    Labor labor;
                    StyleLabor sl;

                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            labor = new Labor(c);
                            db.Labors.Add(labor);
                            sl = new StyleLabor() { StyleId = svm.Style.Id, LaborId = labor.Id };
                            db.StyleLabors.Add(sl);
                            break;
                        case SVMStateEnum.Deleted:
                            sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
                            db.StyleLabors.Remove(sl);
                            break;
                        case SVMStateEnum.Dirty:
                            labor = db.Labors.Find(c.Id);
                            labor.Set(c);
                            /*
                            sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
                            */
                            break;
                        case SVMStateEnum.Clean: // No updates
                        default:
                            break;
                    }
                }
            }

            // Misc
            if (svm.Miscs != null)
            {
                Misc misc;
                StyleMisc sm;
                foreach (MiscComponent c in svm.Miscs)
                {
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            misc = new Misc(c);
                            db.Miscs.Add(misc);
                            sm = new StyleMisc() { StyleId = svm.Style.Id, MiscId = misc.Id };
                            db.StyleMiscs.Add(sm);
                            break;
                        case SVMStateEnum.Deleted:
                            sm = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id && x.MiscId == c.Id).Single();
                            db.StyleMiscs.Remove(sm);
                            break;
                        case SVMStateEnum.Dirty:
                           
                            misc = db.Miscs.Find(c.Id);
                            misc.Set(c);
                            /*
                            sm = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id && x.MiscId == c.Id).Single();
                            */
                            break;
                        case SVMStateEnum.Clean: // No updates
                        default:
                            break;
                    }
                }

                if (svm.SVMState == SVMStateEnum.Added)
                {
                    db.Styles.Add(svm.Style);
                }
            } // false
            if (ModelState.IsValid)
            {
                if (true) // if the modelstate only has validation errors on "Clean" components, then allow the DB update
                {
                    // Save changes, go to Home
                    db.SaveChanges();
                    return RedirectToAction("Index", new { CollectionID = svm.Style.CollectionId });
                }
            }
            Collection co = db.Collections.Find(svm.Style.CollectionId);
            svm.CompanyId = co.CompanyId;
            svm.PopulateDropDownData(db);
            svm.PopulateDropDowns(db);
            svm.RepopulateComponents(db); // iterate thru the data and repopulate the links
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", svm.Style.MetalWtUnitId);
            // iterate thru modelstate errors, display on page
            return View(svm);
        }

        // GET: Styles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
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
                return RedirectToAction("Index", "Home");
            }

            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
            // Move a Style in or out of Memo

            MemoViewModel m = new MemoViewModel();
            m.style = new StyleModel()
            {
                Id = style.Id,
                Name = style.StyleName,
                Num = style.StyleNum,
                Qty = style.Quantity,
                Memod = style.Memos.Sum(s => s.Quantity)
            };
            
            m.Memos = new List<MemoModel>();
            m.numPresentersWithStyle = 0;
            
            foreach (Memo i in db.Memos.Where(x => x.StyleID == style.Id).Include(x => x.Presenter))
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
            m.NewExistingPresenterRadio = 2;
            foreach (Presenter i in db.Presenters.Where(w => w.CompanyId == m.CompanyId))
            {
                SelectListItem sli = new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                };
                m.Presenters.Add(sli);
                // select only new presenter if there aren't any presenters
                m.NewExistingPresenterRadio = 1;
            }
            m.SendReturnMemoRadio = 1;
            m.PresenterName = "";
            
            ViewBag.CollectionId = style.CollectionId;
            return View(m);
        }

        [HttpPost]
        public ActionResult Memo(MemoViewModel m)
        {
            ModelState.Clear();
            OJewelryDB dc = new OJewelryDB();
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
                    if (memo.ReturnQty < 0)
                    {
                        ModelState.AddModelError("Return Style", "You can only return a positive number to inventory.");
                    }
                    if (memo.ReturnQty > 0)
                    {
                        if (memo.ReturnQty > memo.Quantity)
                        {
                            ModelState.AddModelError("Return Style", "You can't return more items than were memo'd out.");
                        }
                        // update db
                        Memo mdb = dc.Memos.Find(memo.Id);
                        if (mdb.Quantity == memo.ReturnQty)
                        {
                            // remove the row and remove item from collection
                            dc.Memos.Remove(mdb);
                        }
                        else
                        {
                            // decrease the amount
                            mdb.Quantity -= memo.ReturnQty;
                        }
                        sdb.Quantity += memo.ReturnQty;
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

        }

        public ActionResult Sell(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Move a Style in or out of Memo
            Style style = db.Styles.Find(id);
            if (style == null)
            {
                return HttpNotFound();
            }
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

        void xPopulateStyleViewModelDropDowns(StyleViewModel svm)
        {
            // reusables
            svm.jsVendors = db.Vendors.ToList();
            svm.jsMetals = db.MetalCodes.ToList();
            svm.jsStones = db.Components.Include("ComponentType").Where(x => x.CompanyId == svm.CompanyId && x.ComponentType.Name == "Stones").ToList();
            svm.jsFindings = db.Components.Include("ComponentType").Where(x => x.CompanyId == svm.CompanyId && x.ComponentType.Name == "Findings").ToList();
            // populate each cost component dropdown in model
        }

        void xGetPresenters(OJewelryDB dc, MemoViewModel m, int CompanyId)
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
