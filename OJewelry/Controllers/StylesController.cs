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
            StyleViewModel svm = new StyleViewModel();
            PopulateStyleViewModel(null, svm);
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
            svm.SVMState = SVMStateEnum.Added;
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
            PopulateStyleViewModel(id, svm);
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
            //db.Entry(svm.Style).State = EntityState.Modified;
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
                        case SVMStateEnum.Clean:
                        default:
                            casting = db.Castings.Find(c.Id);
                            casting.Set(c);
                            // Update the Syle-Casting Link
                            sc = db.StyleCastings.Where(x => x.StyleId == svm.Style.Id && x.CastingId == c.Id).SingleOrDefault();
                            break;
                    }
                }
            }

            // Stones
            if (svm.Stones != null)
            {
                foreach (StoneComponent c in svm.Stones)
                {
                    Component component;
                    StyleComponent sc;

                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            component = new Component(c);
                            db.Components.Add(component);
                            sc = new StyleComponent() { StyleId = svm.Style.Id, ComponentId = component.Id };
                            sc.Quantity = c.Qty;
                            db.StyleComponents.Add(sc);
                            break;
                        case SVMStateEnum.Deleted:
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                            db.StyleComponents.Remove(sc);
                            break;
                        default:
                        case SVMStateEnum.Clean:
                        case SVMStateEnum.Dirty:
                            component = db.Components.Find(c.Id);
                            component.Set(c);
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                            sc.Quantity = c.Qty;
                            break;
                    }
                }
            }

            // Findings
            if (svm.Findings != null)
            {
                foreach (FindingsComponent c in svm.Findings)
                {
                    Component component;
                    StyleComponent sc;
                    switch (c.SVMState)
                    {
                        case SVMStateEnum.Added:
                            component = new Component(c);
                            db.Components.Add(component);
                            sc = new StyleComponent() { StyleId = svm.Style.Id, ComponentId = component.Id };
                            sc.Quantity = c.Qty;
                            db.StyleComponents.Add(sc);
                            break;
                        case SVMStateEnum.Deleted:
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                            db.StyleComponents.Remove(sc);
                            break;
                        default:
                        case SVMStateEnum.Clean:
                        case SVMStateEnum.Dirty:
                            component = db.Components.Find(c.Id);
                            component.Set(c);
                            sc = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id && x.ComponentId == c.Id).Single();
                            sc.Quantity = c.Qty;
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
                        default:
                        case SVMStateEnum.Clean:
                        case SVMStateEnum.Dirty:
                            labor = db.Labors.Find(c.Id);
                            labor.Set(c);
                            sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
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
                        default:
                        case SVMStateEnum.Clean:
                        case SVMStateEnum.Dirty:
                            misc = db.Miscs.Find(c.Id);
                            misc.Set(c);
                            sm = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id && x.MiscId == c.Id).Single();
                            break;
                    }
                }

                if (svm.SVMState == SVMStateEnum.Added)
                {
                    db.Styles.Add(svm.Style);
                }
                if (ModelState.IsValid)
                {
                    // Save changes, go to Home
                    db.SaveChanges();
                    return RedirectToAction("Index", new { CollectionID = svm.Style.CollectionId });
                }
            } // false
            Collection co = db.Collections.Find(svm.Style.CollectionId);
            svm.CompanyId = co.CompanyId;
            PopulateStyleViewModelDropDowns(svm);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", svm.Style.MetalWtUnitId);
            // iterate thru modelstate errors, display on page
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

        void PopulateStyleViewModelDropDowns(StyleViewModel svm)
        {
            svm.jsVendors = new SelectList(db.Vendors, "Id", "Name", db.Vendors.First().Id);
            svm.jsMetals = new SelectList(db.MetalCodes, "Id", "Code", db.MetalCodes.First().Id);
            List<Component> stones = db.Components.Include("ComponentType").Where(x => x.CompanyId == svm.CompanyId && x.ComponentType.Name == "Stones").ToList();
            List<Component> findings = db.Components.Include("ComponentType").Where(x => x.CompanyId == svm.CompanyId && x.ComponentType.Name == "Findings").ToList();
            svm.jsStones = new SelectList(stones, "Id", "Name", stones.First().Id);
            svm.jsFindings = new SelectList(findings, "Id", "Name", findings.First().Id);
        }

        void PopulateStyleViewModel(int? id, StyleViewModel svm)
        {
            decimal t = 0, t2 = 0;
            svm.Castings = new List<CastingComponent>();
            svm.Stones = new List<StoneComponent>();
            svm.Findings = new List<FindingsComponent>();
            svm.Labors = new List<LaborComponent>();
            svm.Miscs = new List<MiscComponent>();

            PopulateStyleViewModelDropDowns(svm);

            if (id == null)
            {
                svm.Style = new Style();
            }
            else
            {
                // Get Casting Links
                svm.Style.StyleCastings = db.StyleCastings.Where(x => x.StyleId == svm.Style.Id).ToList();
                // Get component links
                svm.Style.StyleComponents = db.StyleComponents.Where(x => x.StyleId == svm.Style.Id).ToList();
                // Get Labor Links
                svm.Style.StyleLabors = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id).ToList(); ;
                // Get Misc Links
                svm.Style.StyleMiscs = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id).ToList(); ;
                // get components for each link
                // Metals
                foreach (StyleCasting sc in svm.Style.StyleCastings)
                {
                    Casting casting = db.Castings.Find(sc.CastingId); // Castings
                    CastingComponent cstc = new CastingComponent(casting);
                    // Need to get the vendor and metal code
                    cstc.VendorId = casting.VendorId.Value;
                    cstc.VendorList = new SelectList(db.Vendors, "Id", "Name", casting.VendorId.Value);
                    cstc.MetalCodes = new SelectList(db.MetalCodes, "Id", "Code", casting.MetalCodeID.Value);
                    //cstc.VendorName = db.Vendors.Find(casting.VendorId).Name; //  Vendor();
                    cstc.MetalCode = db.MetalCodes.Find(casting.MetalCodeID).Code; // Metal Code
                    cstc.Qty = casting.Qty.Value;
                    t = cstc.Price ?? 0;
                    t2 = cstc.Labor ?? 0;
                    cstc.Total = cstc.Qty * (t + t2);
                    svm.MetalsTotal += cstc.Total;
                    svm.Castings.Add(cstc);
                    svm.Total += cstc.Total;
                }
                // Stones and Findings
                foreach (StyleComponent sc in svm.Style.StyleComponents)
                {
                    Component Comp = db.Components.Find(sc.ComponentId); // Stones and Findings
                    switch (sc.Component.ComponentType.Id)
                    {
                        case 1: // Metals (Castings)
                            break;

                        case 2: // Stones
                            Comp.Vendor = db.Vendors.Find(Comp.VendorId) ?? new Vendor();
                            StoneComponent stscm = new StoneComponent(Comp);
                            stscm.VendorId = Comp.VendorId.Value;
                            //stscm.VendorName = Comp.Vendor.Name;
                            stscm.Qty = sc.Quantity ?? 0;
                            StoneComponent.componentsForCompany = StoneComponent.GetAvailComps(svm.CompanyId);
                            //stscm.ac = new SelectList(StoneComponent.componentsForCompany, "id", "Name", sc.ComponentId);
                            t = stscm.PPC ?? 0;
                            stscm.Total = stscm.Qty * t;
                            svm.StonesTotal += stscm.Total;
                            svm.Stones.Add(stscm);
                            svm.Total += stscm.Total;
                            break;
                        case 3: // Findings
                            Comp.Vendor = db.Vendors.Find(Comp.VendorId) ?? new Vendor();
                            FindingsComponent fiscm = new FindingsComponent(Comp);
                            fiscm.VendorId = Comp.VendorId.Value;
                            //fiscm.VendorName = Comp.Vendor.Name;
                            fiscm.Metal = db.MetalCodes.Find(Comp.MetalCodeId).Code;
                            //fiscm.MetalCodes = new SelectList(db.MetalCodes, "Id", "Code", Comp.MetalCodeId.Value);
                            fiscm.Qty = sc.Quantity ?? 0;
                            FindingsComponent.componentsForCompany = FindingsComponent.GetAvailComps(svm.CompanyId);
                            //fiscm.ac = new SelectList(FindingsComponent.componentsForCompany, "id", "Name", sc.ComponentId);
                            t = fiscm.Price ?? 0;
                            fiscm.Total = fiscm.Qty * t;
                            svm.FindingsTotal += fiscm.Total;
                            svm.Findings.Add(fiscm);
                            svm.Total += fiscm.Total;
                            break;
                        case 4:  // Labor
                            break;
                        default: // Misc
                            break;
                    }
                }
                // Labor
                foreach (StyleLabor sl in svm.Style.StyleLabors)
                {
                    Labor lb = db.Labors.Find(sl.LaborId); // Stones and Findings
                    LaborComponent liscm = new LaborComponent(lb);
                    liscm.Qty = sl.Labor.Qty ?? 0;
                    t = liscm.PPH ?? 0;
                    t2 = liscm.PPP ?? 0;
                    liscm.Total = liscm.Qty.Value * (t + t2);
                    svm.LaborsTotal += liscm.Total;
                    svm.Labors.Add(liscm);
                    svm.Total += liscm.Total;
                }
                // Misc
                foreach (StyleMisc sms in svm.Style.StyleMiscs)
                {
                    Misc misc = db.Miscs.Find(sms.MiscId); // Stones and Findings
                    MiscComponent miscm = new MiscComponent(misc);
                    miscm.Qty = sms.Misc.Qty ?? 0;
                    t = miscm.PPP ?? 0;
                    miscm.Total = miscm.Qty.Value * t;
                    svm.MiscsTotal += miscm.Total;
                    svm.Miscs.Add(miscm);
                    svm.Total += miscm.Total;
                }
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
