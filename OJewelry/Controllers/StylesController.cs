using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage.Blob;
using OJewelry.Classes;
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
            sm.SVMOp = SVMOperation.Print;
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
            AddDefaultEntries(svm);
            return CreateNew(svm.CompanyId, collectionId, svm);
        }

        public ActionResult CreateNew(int companyId, int collectionId, StyleViewModel svm)
        {
            svm.Style.Collection = db.Collections.Find(collectionId);
            svm.Style.CollectionId = collectionId;
            svm.CompanyId = svm.Style.Collection.CompanyId;

            svm.PopulateDropDownData(db);
            svm.PopulateDropDowns(db);
            svm.RepopulateComponents(db); // iterate thru the data and repopulate the links

            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == svm.CompanyId), "Id", "Name", svm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", svm.Style.MetalWtUnitId);
            return View("Create", svm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(StyleViewModel svm)
        {
            ModelState.Clear();
            StyleViewModel newsvm = new StyleViewModel(svm);
            newsvm.SVMOp = SVMOperation.Create;
            newsvm.Style.Collection = db.Collections.Find(newsvm.Style.CollectionId);
            newsvm.CompanyId = newsvm.Style.Collection.CompanyId;

            newsvm.PopulateDropDownData(db);
            newsvm.PopulateDropDowns(db);
            //newsvm.RepopulateComponents(db); // iterate thru the data and repopulate the links

            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == newsvm.CompanyId), "Id", "Name", newsvm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", newsvm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", newsvm.Style.MetalWtUnitId);
            return View(newsvm);
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,StyleNum,StyleName,Desc,JewelryTypeId,CollectionId,IntroDate,Image,Width,Length,ChainLength,RetailRatio,RedlineRatio,Quantity")] Style style)
        public Task <ActionResult> Create(StyleViewModel svm)
        {
            svm.SVMOp = SVMOperation.Create;
            int i = db.Styles.
                Join(db.Collections, s => s.CollectionId, col => col.Id, (s, c) => new
                {
                    StyleId = s.Id,
                    StyleNum = s.StyleNum,
                    StyleName = s.StyleName,
                    CompanyId = c.CompanyId,
                }).Where(x => x.CompanyId == svm.CompanyId  && (x.StyleNum == svm.Style.StyleNum || x.StyleName == svm.Style.StyleName)).Count();
            if (i != 0) // is there a style with the same number for this company?
            {
                ModelState.AddModelError("Style.StyleNum", "Style with this number/name already exists for "
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
            svm.Style.MetalWeightUnit = new MetalWeightUnit
            {
                Unit = "DWT"
            };
            svm.Populate(id, db);
            MarkDefaultEntriesAsFixed(svm);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes, "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits, "Id", "Unit", svm.Style.MetalWtUnitId);
            return View(svm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StyleViewModel svm)
        {
            //ModelState.Clear();
            int i;
            // Save the Style and all edited components; add the new ones and remove the deleted ones
            if (db.Entry(svm.Style).State != EntityState.Added) db.Entry(svm.Style).State = EntityState.Modified;
            if (ModelState.IsValid)
            {
                // Iterate thru the components
                // Castings
                if (svm.Castings != null)
                {
                    i = -1;
                    foreach (CastingComponent c in svm.Castings)
                    {
                        i++;
                        Casting casting;
                        StyleCasting sc;
                        try
                        {
                            ValidCasting(c);
                        }
                        catch (OjMissingCastingException e)
                        {
                            ModelState.AddModelError("Castings[" + i + "].Name", e.Message);
                            continue;
                        }

                        switch (c.SVMState)
                        {
                            case SVMStateEnum.Added:
                                casting = new Casting(c);
                                // add a new link
                                casting.Id = -i;
                                db.Castings.Add(casting);
                                sc = new StyleCasting()
                                {
                                    CastingId = casting.Id,
                                    StyleId = svm.Style.Id,
                                };
                                db.StyleCastings.Add(sc);
                                break;
                            case SVMStateEnum.Deleted:
                                sc = db.StyleCastings.Where(x => x.StyleId == svm.Style.Id && x.CastingId == c.Id)
                                    .SingleOrDefault();
                                casting = db.Castings.Find(c.Id);
                                db.StyleCastings.Remove(sc);
                                db.Castings.Remove(casting);
                                break;
                            case SVMStateEnum.Dirty:
                            case SVMStateEnum.Fixed:
                                casting = db.Castings.Find(c.Id);
                                casting.Set(c);
                                /*
                                // Update the Syle-Casting Link
                                sc = db.StyleCastings.Where(x => x.StyleId == svm.Style.Id && x.CastingId == c.Id).SingleOrDefault();
                                */
                                break;
                            case SVMStateEnum.Unadded:
                                break;
                            default:
                                break;
                        }
                    }
                }
                // Stones
                if (svm.Stones != null)
                {
                    i = -1;
                    foreach (StoneComponent sc in svm.Stones)
                    {
                        i++;
                        //Stone stone;
                        StyleStone ss;
                        int stoneId = 0;
                        try
                        {
                            stoneId = ValidStone(sc);
                        }
                        catch (OjInvalidStoneComboException e)
                        {
                            ModelState.AddModelError("Stones[" + i + "].Name", e.Message);
                            continue;
                        }
                        catch (OjMissingStoneException e)
                        {
                            ModelState.AddModelError("Stones[" + i + "].Name", e.Message);
                            continue;
                        }

                        switch (sc.SVMState)
                        {
                            case SVMStateEnum.Added:
                                //stone = new Stone(sc);
                                //db.Stones.Add(stone);
                                ss = new StyleStone()
                                {
                                    StyleId = svm.Style.Id,
                                    StoneId = stoneId,
                                    Qty = sc.Qty
                                };
                                db.StyleStones.Add(ss);
                                break;
                            case SVMStateEnum.Deleted:
                                ss = db.StyleStones.Where(x => x.Id == sc.linkId).SingleOrDefault();
                                db.StyleStones.Remove(ss);
                                break;
                            case SVMStateEnum.Dirty:
                            case SVMStateEnum.Fixed:
                                //stone = db.Stones.Find(sc.Id);
                                //stone.Set(sc);
                                ss = db.StyleStones.Where(x => x.Id == sc.linkId).SingleOrDefault();
                                //ss = db.StyleStones.Where(x => x.StyleId == svm.Style.Id && x.Id == sc.Id).SingleOrDefault();
                                ss.Qty = sc.Qty;
                                ss.StoneId = stoneId;
                                break;
                            case SVMStateEnum.Unadded:
                            default:
                                break;
                        }
                    }
                }
                // Findings
                if (svm.Findings != null)
                {
                    i = -1;
                    foreach (FindingsComponent c in svm.Findings)
                    {
                        i++;
                        StyleFinding fc;
                        try
                        {
                            ValidFinding(c, i);
                        }
                        catch (OjMissingFindingException e)
                        {
                            ModelState.AddModelError("Findings[" + i + "].Id", e.Message);
                            continue;
                        }
                        switch (c.SVMState)
                        {
                            case SVMStateEnum.Added:
                                /*
                                component = new Component(c);
                                db.Components.Add(component);
                                */
                                fc = new StyleFinding()
                                {
                                    StyleId = svm.Style.Id,
                                    FindingId = c.Id ?? 0,
                                    Qty = c.Qty
                                };

                                db.StyleFindings.Add(fc);
                                break;
                            case SVMStateEnum.Deleted:
                                fc = db.StyleFindings.Where(x => x.Id == c.linkId).SingleOrDefault();
                                db.StyleFindings.Remove(fc);
                                break;
                            case SVMStateEnum.Dirty:
                            case SVMStateEnum.Fixed:
                                /*
                                component = db.Components.Find(c.Id);
                                component.Set(c);
                                */
                                //finding.Set(c); // Dont change the finding, just the link!!!
                                fc = db.StyleFindings.Where(x => x.Id == c.linkId).SingleOrDefault();
                                fc.FindingId = c.Id ?? 0;
                                fc.Qty = c.Qty;
                                break;
                            case SVMStateEnum.Unadded: // No updates
                            default:
                                break;
                        }
                    }
                }
                // Labors
                if (svm.Labors != null)
                {
                    i = -1;
                    foreach (LaborComponent c in svm.Labors)
                    {
                        i++;
                        Labor labor;
                        StyleLabor sl;
                        try
                        {
                            ValidLabor(c);
                        }
                        catch (OjMissingLaborException e)
                        {
                            ModelState.AddModelError("Labors[" + i + "].Name", e.Message);
                            continue;
                        }

                        switch (c.SVMState)
                        {
                            case SVMStateEnum.Added:
                                AddLabor(c, svm, i);
                                break;
                            case SVMStateEnum.Deleted:
                                sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
                                db.StyleLabors.Remove(sl);
                                break;
                            case SVMStateEnum.Dirty:
                            case SVMStateEnum.Fixed:
                                if (c.Id <= 0)
                                {
                                    AddLabor(c, svm, i);
                                }
                                else
                                {
                                    labor = db.Labors.Find(c.Id);
                                    labor.Set(c);
                                }

                                /*
                                sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
                                */
                                break;
                            case SVMStateEnum.Unadded: // No updates
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
                    i = -1;
                    foreach (MiscComponent c in svm.Miscs)
                    {
                        i++;
                        try
                        {
                            ValidMisc(c);
                        }
                        catch (OjMissingMiscException e)
                        {
                            ModelState.AddModelError("Miscs[" + i + "].Name", e.Message);
                            continue;
                        }

                        switch (c.SVMState)
                        {
                            case SVMStateEnum.Added:
                                AddMisc(c, svm, i);
                                break;
                            case SVMStateEnum.Deleted:
                                sm = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id && x.MiscId == c.Id).Single();
                                db.StyleMiscs.Remove(sm);
                                break;
                            case SVMStateEnum.Dirty:
                            case SVMStateEnum.Fixed:
                                if (c.Id == 0)
                                {
                                    AddMisc(c, svm, i);
                                }
                                else
                                {
                                    misc = db.Miscs.Find(c.Id);
                                    misc.Set(c);
                                }

                                /*
                                sm = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id && x.MiscId == c.Id).Single();
                                */
                                break;
                            case SVMStateEnum.Unadded: // No updates
                            default:
                                break;
                        }
                    }

                    if (svm.SVMState == SVMStateEnum.Added)
                    {
                        db.Styles.Add(svm.Style);
                    }
                } // false
            }
            await SaveImageInStorage(svm);
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
            // remove memos 
            List<Memo> rmMemos = db.Memos.Where(m => m.StyleID == id).ToList();
            db.Memos.RemoveRange(rmMemos);
            db.Styles.Remove(style);
            db.SaveChanges();
            return RedirectToAction("Index", new { CollectionID = collectionId });
        }

        public bool ValidCasting(CastingComponent cc)
        {
            if (cc.SVMState == SVMStateEnum.Unadded) return true;
            if (string.IsNullOrEmpty(cc.Name))
            {
                throw new OjMissingCastingException("You must enter a Name!");
            }

            return true;
        }

        public int ValidStone(StoneComponent sc)
        {
            if (sc.SVMState != SVMStateEnum.Unadded)
            {
                // Make sure a stone was selected in the dropdown
                if (sc.Name == null)
                {
                    throw new OjMissingStoneException("You must select a stone!");
                }

                // Ensure combo of stone, shape, size is valid (db.stone.where...)
                Stone stone = db.Stones
                    .Where(st => st.Name == sc.Name && st.Shape.Name == sc.ShId && st.StoneSize == sc.SzId)
                    .FirstOrDefault();
                if (stone == null)
                {
                    throw new OjInvalidStoneComboException("Invalid Stone combination!");
                }

                return stone.Id;
            }
            return 0;
        }

        public bool ValidFinding(FindingsComponent fc, int i)
        {
            // Make sure a stone was selected in the dropdown
            if (fc.SVMState == SVMStateEnum.Unadded) return true;
            if (fc.Id == 0)
            {
                throw new OjMissingFindingException("You must enter a Name!");
            }
            return true;
        }

        public bool ValidLabor(LaborComponent lc)
        {
            if (lc.SVMState == SVMStateEnum.Unadded) return true;
            if (string.IsNullOrEmpty(lc.Name))
            {
                throw new OjMissingLaborException("You must enter a Name!");
            }

            return true;
        }

        public bool ValidMisc(MiscComponent mc)
        {
            if (mc.SVMState == SVMStateEnum.Unadded) return true;
            if (string.IsNullOrEmpty(mc.Name))
            {
                throw new OjMissingMiscException("You must enter a Name!");
            }

            return true;
        }

        public void AddDefaultEntries(StyleViewModel svm)
        {
            // Add 2 Fixed Labor entries and 1 Fixed Misc entry
            LaborComponent lc = new LaborComponent
            {
                Id = -1,
                Name = "FINISHING LABOR",
                SVMState = SVMStateEnum.Fixed
            };
            svm.Labors.Add(lc);
            lc = new LaborComponent
            {
                Id = -2,
                Name = "SETTING LABOR",
                SVMState = SVMStateEnum.Fixed
            };
            svm.Labors.Add(lc);//svm.Stones.Add(new StoneComponent(new Stone()));
            MiscComponent mc = new MiscComponent
            {
                Name = "PACKAGING",
                SVMState = SVMStateEnum.Fixed,
                Qty = 1
            };
            svm.Miscs.Add(mc);
        }

        public void MarkDefaultEntriesAsFixed(StyleViewModel svm)
        {
            if (svm.Labors != null && svm.Labors.Count > 1)
            {
                if (svm.Labors[0].Name == "FINISHING LABOR")
                {
                    svm.Labors[0].SVMState = SVMStateEnum.Fixed;
                    svm.Labors[1].SVMState = SVMStateEnum.Fixed;
                }
            }

            if (svm.Miscs != null && svm.Miscs.Count > 0)
            {
                if (svm.Miscs[0].Name == "PACKAGING")
                {
                    svm.Miscs[0].SVMState = SVMStateEnum.Fixed;
                }
            }

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

        /*
        void xPopulateStyleViewModelDropDowns(StyleViewModel svm)
        {
            // reusables
            svm.jsVendors = db.Vendors.ToList();
            svm.jsMetals = db.MetalCodes.ToList();
            svm.jsStones = db.Stones.Where(x => x.CompanyId == svm.CompanyId).ToList();
            svm.jsShapes = svm.jsStones.Select(x=> x.).ToList();
            svm.jsSizes = db.Sizes.Where(x => x.CompanyId == svm.CompanyId).ToList();
            svm.jsFindings = db.Findings.Where(x => x.CompanyId == svm.CompanyId).ToList();
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
        */

        void AddLabor(LaborComponent c, StyleViewModel svm, int keyVal)
        {
            Labor labor = new Labor(c);
            labor.Id = c.Id == 0 ? keyVal : c.Id;
            db.Labors.Add(labor);
            StyleLabor sl = new StyleLabor() { StyleId = svm.Style.Id, LaborId = labor.Id };
            db.StyleLabors.Add(sl);
        }

        void AddMisc(MiscComponent m, StyleViewModel svm, int keyVal)
        {
            Misc misc = new Misc(m);
            misc.Id = m.Id == 0 ? keyVal : m.Id;
            db.Miscs.Add(misc);
            StyleMisc sm = new StyleMisc() { StyleId = svm.Style.Id, MiscId = misc.Id };
            db.StyleMiscs.Add(sm);
        }

        private async Task<bool> SaveImageInStorage(StyleViewModel svm)
        {
            //AzureBlobStorageContainer container = new AzureBlobStorageContainer();
            //await container.Init(ojStoreConnStr, "ojewelry");
            if (svm.PostedImageFile != null)
            {
                svm.Style.Image = await Singletons.azureBlobStorage.Upload(svm.PostedImageFile);
            }

            return true;
        }

        /*
        private async Task<bool> RetrieveImageFromStorage(StyleViewModel svm)
        {
            return true;
        }
        */

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
