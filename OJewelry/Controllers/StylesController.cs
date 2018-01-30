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
        private OJewelryDB db = new OJewelryDB();

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
            PopulateStyleViewModel(id, sm);
            return View(sm);
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
            PopulateStyleViewModel(id, sm);
            return View(sm);
        }

        // GET: Styles/Create
        public ActionResult Create(int collectionId)
        {
            Collection co = db.Collections.Find(collectionId);
            Style s = new Style()
            {
                CollectionId = collectionId,
            };
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name");
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit");
            //ViewBag.JewelryTypes = new SelectList(db.JewelryTypes);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", s.JewelryTypeId);
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
            ViewBag.CollectionId = new SelectList(db.Collections, "Id", "Name", style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", style.JewelryTypeId);
            style.Collection = db.Collections.Find(style.CollectionId);
            return View(style);
        }

        // GET: Styles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            StyleViewModel sm = new StyleViewModel();
            sm.Style = db.Styles.Find(id);
            if (sm.Style == null)
            {
                return HttpNotFound();
            }
            PopulateStyleViewModel(id, sm);
            Collection co = db.Collections.Find(sm.Style.CollectionId);
            sm.CompanyId = co.CompanyId;
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", sm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", sm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", sm.Style.MetalWtUnitId);
            return View(sm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StyleViewModel svm)
        {
            ModelState.Clear();
            // Save the Style and all edited components; add the new ones and remove the deleted ones
            db.Entry(svm.Style).State = EntityState.Modified;
            // Iterate thru the components
            /*
            if (svm.Metals != null)
            {
                
                foreach (MetalCode c in svm.Metals)
                {
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            break;
                        case SVMStateEnum.Clean:
                            break;
                        case SVMStateEnum.Deleted:
                            break;
                        case SVMStateEnum.Dirty:
                            db.Entry(c.Comp).State = EntityState.Modified;
                            break;
                    }
                    // Update the Syle-Component Link
                    StyleComponent sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                    sc.Quantity = c.Qty;
                }
            }
            */
            if (svm.Stones != null)
            {
                foreach (StoneComponent c in svm.Stones)
                {
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            break;
                        case SVMStateEnum.Clean:
                            break;
                        case SVMStateEnum.Deleted:
                            break;
                        case SVMStateEnum.Dirty:
                            db.Entry(c.Comp).State = EntityState.Modified;
                            break;
                    }
                    // Update the Syle-Component Link
                    StyleComponent sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                    sc.Quantity = c.Qty;
                }
            }
            if (svm.Findings != null)
            {
                foreach (FindingsComponent c in svm.Findings)
                {
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            break;
                        case SVMStateEnum.Clean:
                            break;
                        case SVMStateEnum.Deleted:
                            break;
                        case SVMStateEnum.Dirty:
                            db.Entry(c.Comp).State = EntityState.Modified;
                            break;
                    }
                    // Update the Syle-Component Link
                    StyleComponent sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                    sc.Quantity = c.Qty;
                }
            }
            /*
            if (svm.Labors != null)
            {
                foreach (LaborComponent c in svm.Labors)
                {
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            break;
                        case SVMStateEnum.Clean:
                            break;
                        case SVMStateEnum.Deleted:
                            break;
                        case SVMStateEnum.Dirty:
                            db.Entry(c.Comp).State = EntityState.Modified;
                            break;
                    }
                    // Update the Syle-Component Link
                    StyleComponent sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                    sc.Quantity = c.Qty;
                }
            }
            if (svm.Miscs != null)
            {
                foreach (MiscComponent c in svm.Miscs)
                {
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            break;
                        case SVMStateEnum.Clean:
                            break;
                        case SVMStateEnum.Deleted:
                            break;
                        case SVMStateEnum.Dirty:
                            db.Entry(c.Comp).State = EntityState.Modified;
                            break;
                    }
                    // Update the Syle-Component Link
                    StyleComponent sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                    sc.Quantity = c.Qty;
                }
            }
            */
            if (ModelState.IsValid)
            {
                // Save changes, go to Home
                db.SaveChanges();
                return RedirectToAction("Index", new { CollectionID = svm.Style.CollectionId });
            }
            Collection co = db.Collections.Find(svm.Style.CollectionId);
            svm.CompanyId = co.CompanyId;
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            ViewBag.JewelryTypes = new SelectList(db.JewelryTypes, "Id", "Name", svm.Style.JewelryTypeId);
            return View(svm);
        }

// POST: Styles/Edit/5
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
/*
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
*/
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

        void PopulateStyleViewModel(int? id, StyleViewModel sm)
        {
            decimal t = 0, t2 = 0;
            sm.Castings = new List<CastingComponent>();
            sm.Stones = new List<StoneComponent>();
            sm.Findings = new List<FindingsComponent>();
            sm.Labors = new List<LaborComponent>();
            sm.Miscs = new List<MiscComponent>();
            // Get Casting Links
            sm.Style.StyleCastings = db.StyleCastings.Where(x => x.StyleId == sm.Style.Id).ToList();
            // Get component links
            sm.Style.StyleComponents = db.StyleComponents.Where(x => x.StyleId == sm.Style.Id).ToList();
            // Get Labor Links
            sm.Style.StyleLabors = db.StyleLabors.Where(x => x.StyleId == sm.Style.Id).ToList(); ;
            // Get Misc Links
            sm.Style.StyleMiscs = db.StyleMiscs.Where(x => x.StyleId == sm.Style.Id).ToList(); ;
            // get components for each link
            // Metals
            foreach (StyleCasting sc in sm.Style.StyleCastings)
            {
                Casting casting = db.Castings.Find(sc.CastingId); // Castings
                CastingComponent cstc = new CastingComponent(casting);
                // Need to get the vendor and metal code
                cstc.VendorName = db.Vendors.Find(casting.VendorId).Name; //  Vendor();
                cstc.MetalCode = db.MetalCodes.Find(casting.MetalCodeID).Code; // Metal Code
                cstc.Qty = casting.Qty.Value;
                t = cstc.Price ?? 0;
                t2 = cstc.Labor ?? 0;
                cstc.Total = cstc.Qty * (t + t2);
                sm.MetalsTotal += cstc.Total;
                sm.Castings.Add(cstc);
                sm.Total += cstc.Total;
            }
            foreach (StyleComponent sc in sm.Style.StyleComponents)
            {
                Component Comp = db.Components.Find(sc.ComponentId); // Stones and Findings
                switch (sc.Component.ComponentType.Id)
                {   
                    case 1: // Metals (Castings)
                        break;
                        
                    case 2: // Stones
                        Comp.Vendor = db.Vendors.Find(Comp.VendorId) ?? new Vendor();
                        StoneComponent stscm = new StoneComponent(Comp);
                        stscm.Qty = sc.Quantity ?? 0;
                        t = stscm.PPC ?? 0;
                        stscm.Total = stscm.Qty * t;
                        sm.StonesTotal += stscm.Total;
                        sm.Stones.Add(stscm);
                        sm.Total += stscm.Total;
                        break;
                    case 3: // Findings
                        Comp.Vendor = db.Vendors.Find(Comp.VendorId) ?? new Vendor();
                        FindingsComponent fiscm = new FindingsComponent(Comp);
                        fiscm.Qty = sc.Quantity ?? 0;
                        t = fiscm.Price ?? 0;
                        fiscm.Total = fiscm.Qty * t;
                        sm.FindingsTotal += fiscm.Total;
                        sm.Findings.Add(fiscm);
                        sm.Total += fiscm.Total;
                        break;
                    case 4:  // Labor
                        break;
                    default: // Misc
                        break;
                }
            }
            // Labor
            foreach (StyleLabor sl in sm.Style.StyleLabors)
            {
                Labor lb = db.Labors.Find(sl.LaborId); // Stones and Findings
                LaborComponent liscm = new LaborComponent(lb);
                liscm.Qty = sl.Labor.Qty ?? 0;
                t = liscm.PPH ?? 0;
                t2 = liscm.PPP ?? 0;
                liscm.Total = liscm.Qty.Value * (t + t2);
                sm.LaborsTotal += liscm.Total;
                sm.Labors.Add(liscm);
                sm.Total += liscm.Total;
            }
            // Misc
            foreach (StyleMisc sms in sm.Style.StyleMiscs)
            {
                Misc misc = db.Miscs.Find(sms.MiscId); // Stones and Findings
                MiscComponent miscm = new MiscComponent(misc);
                miscm.Qty = sms.Misc.Qty ?? 0;
                t = miscm.PPP ?? 0;
                miscm.Total = miscm.Qty.Value * t;
                sm.MiscsTotal += miscm.Total;
                sm.Miscs.Add(miscm);
                sm.Total += miscm.Total;
            }
        }

        void GetPresenters(OJewelryDB dc, MemoViewModel m, int CompanyId)
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
