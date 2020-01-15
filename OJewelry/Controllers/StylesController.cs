using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
            var styles = db.Styles.Include(s => s.Collection).Where(i => i.CollectionId == CollectionId).OrderBy(s => s.StyleName).Include(s => s.JewelryType);
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
            //ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == companyId), "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits.OrderBy(mwu => mwu.Unit), "Id", "Unit", svm.Style.MetalWtUnitId);
            return View("Create", svm);
        }

        public void AddDefaultEntries(StyleViewModel svm)
        {
            // Get the cost data and find initial values for the Finishing Labor & Packaging Costs. Both are based on the style's jewelry type
            decimal flPrice = 0;
            decimal packPrice = 0;

            JewelryType jt = db.JewelryTypes.Where(j => svm.CompanyId == j.CompanyId).FirstOrDefault();
            if (jt != null)
            {
                flPrice = jt.FinishingCost;
                packPrice = jt.PackagingCost;
            }

            // Add 2 Fixed Labor entries and 1 Fixed Misc entry
            LaborComponent lc = new LaborComponent
            {
                Id = -1,
                Name = StyleViewModel.FinishingLaborName,
                State = LMState.Fixed,
                PPP = flPrice,
                Qty = 1,
                Total = flPrice
            };
            svm.Labors.Add(lc);
            MiscComponent mc = new MiscComponent
            {
                Id = -1,
                Name = StyleViewModel.PackagingName,
                State = SVMStateEnum.Fixed,
                PPP = packPrice,
                Qty = 1,
                Total = packPrice
            };
            svm.Miscs.Add(mc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Copy(StyleViewModel svm)
        {
            ModelState.Clear();
            svm.SVMOp = SVMOperation.Create;
            StyleViewModel newsvm = new StyleViewModel(svm, db);
            await SaveImageInStorage(db, newsvm, true);

            newsvm.Style.Collection = db.Collections.Find(newsvm.Style.CollectionId);
            newsvm.CompanyId = newsvm.Style.Collection.CompanyId;
            newsvm.CopiedStyleName = svm.Style.StyleName;

            newsvm.PopulateDropDownData(db);
            newsvm.PopulateDropDowns(db);
            newsvm.LookupComponents(db); // iterate thru the data and repopulate the data

            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == newsvm.CompanyId), "Id", "Name", newsvm.Style.CollectionId);
            //ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == newsvm.CompanyId), "Id", "Name", newsvm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits.OrderBy(mwu => mwu.Unit), "Id", "Unit", newsvm.Style.MetalWtUnitId);
            return View(newsvm);
        }

        private bool CheckForNameAndNumberUniqueness(StyleViewModel svm)
        {
            // make sure the collection is valid
            if (svm.Style.Collection == null)
            {
                svm.Style.Collection = db.Collections.Find(svm.Style.CollectionId);
            }
            int iStyleNums = db.Styles.
                Join(db.Collections, s => s.CollectionId, col => col.Id, (s, c) => new
                {
                    StyleId = s.Id,
                    StyleNum = s.StyleNum,
                    StyleName = s.StyleName,
                    CompanyId = c.CompanyId,
                }).Where(x => x.CompanyId == svm.CompanyId
                    && x.StyleId != svm.Style.Id
                    && (x.StyleNum == svm.Style.StyleNum)).Count();
            if (iStyleNums != 0)
            {
                ModelState.AddModelError("Style.StyleNum", "Style with this number already exists for " + db.FindCompany(svm.CompanyId).Name + ".");
            }

            if (iStyleNums != 0)
            {
                return false;
            }
            return true;
        }

        // POST: Styles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Name,StyleNum,StyleName,Desc,JewelryTypeId,CollectionId,IntroDate,Image,Width,Length,ChainLength,RetailRatio,RedlineRatio,Quantity")] Style style)
        public async Task<ActionResult> Create(StyleViewModel svm)
        {
            svm.SVMOp = SVMOperation.Create;
            bool b = CheckForNameAndNumberUniqueness(svm);
            if (b) // is there a style with the same number for this company?
            {
                // No, add it
                svm.SVMState = SVMStateEnum.Added;
            }
            return await Edit(svm);
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
            svm.Style.JewelryType = db.JewelryTypes.Find(svm.Style.JewelryTypeId);
            Collection co = db.Collections.Find(svm.Style.CollectionId);
            svm.CompanyId = co.CompanyId;
            svm.Style.MetalWeightUnit = new MetalWeightUnit
            {
                Unit = "DWT"
            };
            svm.Populate(id, db);
            string markup = db.FindCompany(svm.CompanyId).markup;
            if (markup == null) markup = "[]";
            svm.markups = JsonConvert.DeserializeObject<List<Markup>>(markup);
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            //ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits.OrderBy(mwu => mwu.Unit), "Id", "Unit", svm.Style.MetalWtUnitId);
            return View(svm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(StyleViewModel svm)
        {
            CheckForNameAndNumberUniqueness(svm);
            int i;
            // check Model State for errors

            CheckModelState(svm.Style.StyleNum);
            // Save the Style and all edited components; add the new ones and remove the deleted ones
            if (ModelState.IsValid)
            {
                bool bUseLaborTable = svm.Style.JewelryType.bUseLaborTable;
                svm.Style.JewelryType = null; // hack to avoid maismatch of JewelryTypeId error!!!

                if (db.Entry(svm.Style).State != EntityState.Added) db.Entry(svm.Style).State = EntityState.Modified;
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

                        switch (c.State)
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
                                db.Castings.Remove(casting);
                                db.StyleCastings.Remove(sc);
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
                            stoneId = ValidStone(svm.CompanyId, sc);
                            sc.Id = stoneId;
                        }
                        catch (OjInvalidStoneComboException e)
                        {
                            ModelState.AddModelError("Stones[" + i + "].Name", e.Message);
                            continue;
                        }                        
                        catch (OjMissingStoneException)
                        {
                            if (sc.Name == null) ModelState.AddModelError("Stones[" + i + "].Name", "You must enter a stone!! ");
                            if (sc.ShId == null) ModelState.AddModelError("Stones[" + i + "].ShId", "You must enter a shape!! ");
                            if (sc.SzId == null) ModelState.AddModelError("Stones[" + i + "].SzId", "You must enter a size!! ");
                            continue;
                        }
                        
                        switch (sc.State)
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
                                //db.Stones.Remove(ss.Stone);
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
                        switch (c.State)
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
                                //db.Findings.Remove(fc.Finding);
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
                if (svm.Labors != null  && bUseLaborTable == false)
                {
                    i = -1;
                    foreach (LaborComponent lc in svm.Labors)
                    {
                        i++;
                        Labor labor;
                        StyleLabor sl;
                        try
                        {
                            ValidLabor(lc);
                        }
                        catch (OjMissingLaborException e)
                        {
                            ModelState.AddModelError("Labors[" + i + "].Name", e.Message);
                            continue;
                        }

                        switch (lc.State)
                        {
                            case LMState.Added:
                                AddLabor(lc, svm, i);
                                break;
                            case LMState.Deleted:
                                sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == lc.Id).Single();
                                RemoveLabor(sl);
                                break;
                            case LMState.Fixed:
                            case LMState.Dirty:
                                if (lc.Id <= 0)
                                {
                                    AddLabor(lc, svm, i);
                                }
                                else
                                {
                                    labor = db.Labors.Find(lc.Id);
                                    labor.Set(lc);
                                }

                                /*
                                sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
                                */
                                break;
                            case LMState.Unadded: // No updates
                            default:
                                break;
                        }
                    }
                }
                // Labor Table
                if (svm.LaborItems != null && bUseLaborTable == true)
                {
                    i = -1;
                    foreach (LaborItemComponent lic in svm.LaborItems)
                    {
                        i++;
                        StyleLaborTableItem sl;
                        try
                        {
                            ValidLaborItem(lic);
                        }
                        catch (OjMissingLaborException e)
                        {
                            ModelState.AddModelError("LaborsItems[" + i + "].Name", e.Message);
                            continue;
                        }

                        switch (lic.State)
                        {
                            case LMState.Added:
                                AddLaborItem(lic, svm, i);
                                break;
                            case LMState.Deleted:
                                sl = db.StyleLaborItems.Where(x => x.Id == lic.linkId).SingleOrDefault();
                                RemoveLaborItem(sl);
                                break;
                            case LMState.Dirty:
                            case LMState.Fixed:
                                if (lic.Id <= 0)
                                {
                                    AddLaborItem(lic, svm, i);
                                }
                                else
                                {
                                    //laborItem = db.LaborTable.Find(lic.laborItemId);
                                    sl = db.StyleLaborItems.Where(x => x.Id == lic.linkId).SingleOrDefault();
                                    sl.Qty = lic.Qty.GetValueOrDefault();
                                    //laborItem.Set(lic);
                                }

                                /*
                                sl = db.StyleLabors.Where(x => x.StyleId == svm.Style.Id && x.LaborId == c.Id).Single();
                                */
                                break;
                            case LMState.Unadded: // No updates
                            default:
                                break;
                        }
                        db.Entry(lic._laborItem).State = EntityState.Detached;
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

                        switch (c.State)
                        {
                            case SVMStateEnum.Added:
                                AddMisc(c, svm, i);
                                break;
                            case SVMStateEnum.Deleted:
                                sm = db.StyleMiscs.Where(x => x.StyleId == svm.Style.Id && x.MiscId == c.Id).Single();
                                RemoveMisc(sm);
                                break;
                            case SVMStateEnum.Dirty:
                            case SVMStateEnum.Fixed:
                                if (c.Id <= 0)
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
            if (ModelState.IsValid)
            {
                if (true) // if the modelstate only has validation errors on "Clean" components, then allow the DB update
                {
                    // Save changes, go to Home
                    try
                    {
                        db.SaveChanges(); // need the styleId for the image name
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError($"Error saving style {svm.Style.StyleNum}, msg: {e.Message}");
                    }
                    Trace.TraceInformation("Operation: {0}, svmId:{1}", svm.SVMOp.ToString(), svm.Style.Id);
                    await SaveImageInStorage(db, svm);
                    db.Entry(svm.Style);
                    db.SaveChanges();
                    if (svm.SVMOp != SVMOperation.Print)
                    {
                        // Redurect to Edit
                        return RedirectToAction("Edit", new { id = svm.Style.Id });
                    }
                    else
                    {
                        return Print(svm.Style.Id);
                    }
                }
            }
            Collection co = db.Collections.Find(svm.Style.CollectionId);
            svm.CompanyId = co.CompanyId;
            svm.Style.JewelryType = db.JewelryTypes.Find(svm.Style.JewelryTypeId);

            //svm.Style.
            string markup = db.FindCompany(svm.CompanyId).markup;
            if (markup == null) markup = "[]";
            svm.markups = JsonConvert.DeserializeObject<List<Markup>>(markup);
            svm.PopulateDropDownData(db);
            svm.PopulateDropDowns(db);
            if (svm.SVMOp == SVMOperation.Create)
            {
                svm.LookupComponents(db);
            }
            else
            {
                svm.RepopulateComponents(db); // iterate thru the data and repopulate the links
            }
            ViewBag.CollectionId = new SelectList(db.Collections.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.CollectionId);
            //ViewBag.JewelryTypeId = new SelectList(db.JewelryTypes.Where(x => x.CompanyId == co.CompanyId), "Id", "Name", svm.Style.JewelryTypeId);
            ViewBag.MetalWtUnitId = new SelectList(db.MetalWeightUnits.OrderBy(mwu => mwu.Unit), "Id", "Unit", svm.Style.MetalWtUnitId);
            // iterate thru modelstate errors, display on page
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
            sm.Style.Collection = db.Collections.Find(sm.Style.CollectionId);
            sm.Style.JewelryType = db.JewelryTypes.Find(sm.Style.JewelryTypeId);

            sm.CompanyId = sm.Style.Collection.CompanyId;
            sm.Populate(id, db);
            sm.SVMOp = SVMOperation.Print;
            return View(sm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Print(StyleViewModel svm)
        {
            // First, save the data
            svm.SVMOp = SVMOperation.Print;
            ModelState.Clear();
            StyleViewModel newsvm = new StyleViewModel(svm, db);
            await SaveImageInStorage(db, newsvm, true);
            //newsvm.assemblyCost.Load(db, svm.CompanyId);
            // Save image in svm as tmp file, assign newsvm.Style.Image to saved image in svm
            newsvm.SVMOp = SVMOperation.Print;
            newsvm.Style.StyleNum = svm.Style.StyleNum;
            newsvm.Style.Collection = db.Collections.Find(newsvm.Style.CollectionId);
            newsvm.CompanyId = newsvm.Style.Collection.CompanyId;
            newsvm.CopiedStyleName = svm.Style.StyleName;
            //svm.RepopulateComponents(db); // iterate thru the data and repopulate the links
            newsvm.Style.JewelryType = db.JewelryTypes.Find(newsvm.Style.JewelryTypeId);
            newsvm.Style.MetalWeightUnit = db.MetalWeightUnits.Find(newsvm.Style.MetalWtUnitId);
            newsvm.PopulateDropDownData(db);
            newsvm.PopulateDropDowns(db);
            newsvm.LookupComponents(db); // iterate thru the data and repopulate the data

            return View(newsvm);
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
            string imageName = style.Image;
            int collectionId = style.CollectionId;
            // remove memos 
            List<Memo> rmMemos = db.Memos.Where(m => m.StyleID == id).ToList();
            db.Memos.RemoveRange(rmMemos);
            {
                // remove components
                db.Castings.RemoveRange(db.StyleCastings.Where(sc => sc.StyleId == id).Select(sc => sc.Casting).ToList());
                db.Stones.RemoveRange(db.StyleStones.Where(ss => ss.StyleId == id).Select(sc => sc.Stone).ToList());
                db.Findings.RemoveRange(db.StyleFindings.Where(sf => sf.StyleId == id).Select(sf => sf.Finding).ToList());
                db.Labors.RemoveRange(db.StyleLabors.Where(sl => sl.StyleId == id).Select(sl => sl.Labor).ToList());
                db.LaborTable.RemoveRange(db.StyleLaborItems.Where(sli => sli.StyleId == id).Select(sli => sli.LaborItem).ToList());
                db.Miscs.RemoveRange(db.StyleMiscs.Where(sm => sm.StyleId == id).Select(sm => sm.Misc).ToList());
                // remove links
                db.StyleCastings.RemoveRange(style.StyleCastings.ToList());
                db.StyleFindings.RemoveRange(style.StyleFindings.ToList());
                db.StyleLaborItems.RemoveRange(style.StyleLaborItems.ToList());
                db.StyleLabors.RemoveRange(style.StyleLabors.ToList());
                db.StyleMiscs.RemoveRange(style.StyleMiscs.ToList());
                db.StyleStones.RemoveRange(style.StyleStones.ToList());
            }
            db.Styles.Remove(style);
            db.SaveChanges();
            RemoveImageFromStorage(imageName);
            return RedirectToAction("Index", new { CollectionID = collectionId });
        }

        public bool ValidCasting(CastingComponent cc)
        {
            if (cc.State == SVMStateEnum.Unadded || cc.State == SVMStateEnum.Deleted) return true;
            if (string.IsNullOrEmpty(cc.Name))
            {
                throw new OjMissingCastingException("You must enter a Name!");
            }

            return true;
        }

        public int ValidStone(int companyId, StoneComponent sc)
        {
            if (sc.State != SVMStateEnum.Unadded && sc.State != SVMStateEnum.Deleted)
            {
                // Make sure a stone was selected in the dropdown
                if (sc.Id == 0)
                {
                    throw new OjMissingStoneException("You must select a stone!");
                }

                return sc.Id.Value;
            }
            return 0;
        }

        public bool ValidFinding(FindingsComponent fc, int i)
        {
            // Make sure a stone was selected in the dropdown
            if (fc.State == SVMStateEnum.Unadded || fc.State == SVMStateEnum.Deleted) return true;
            if (fc.Id == 0)
            {
                throw new OjMissingFindingException("You must enter a Name!");
            }
            return true;
        }

        public bool ValidLabor(LaborComponent lc)
        {
            if (lc.State == LMState.Unadded || lc.State == LMState.Deleted) return true;
            if (string.IsNullOrEmpty(lc.Name))
            {
                throw new OjMissingLaborException("You must enter a Name!");
            }

            return true;
        }

        public bool ValidLaborItem(LaborItemComponent lic)
        {
            if (lic.State == LMState.Unadded || lic.State == LMState.Deleted) return true;
            if (string.IsNullOrEmpty(lic.Name))
            {
                throw new OjMissingLaborException("You must enter a Name!");
            }

            return true;
        }

        public bool ValidMisc(MiscComponent mc)
        {
            if (mc.State == SVMStateEnum.Unadded || mc.State == SVMStateEnum.Deleted) return true;
            if (string.IsNullOrEmpty(mc.Name))
            {
                throw new OjMissingMiscException("You must enter a Name!");
            }

            return true;
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
                Image = style.Image,
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
                Image = style.Image,
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

        void AddLaborItem(LaborItemComponent c, StyleViewModel svm, int keyVal)
        {
            int laborItemId = c.Id == 0 ? keyVal : c.Id;
            StyleLaborTableItem sl = new StyleLaborTableItem() {
                StyleId = svm.Style.Id,
                LaborTableId = c.laborItemId,
                Qty = c.Qty.Value
            };
            db.StyleLaborItems.Add(sl);
        }

        void RemoveLabor(StyleLabor sl)
        {
            db.Labors.Remove(sl.Labor);
            db.StyleLabors.Remove(sl);
        }

        void RemoveLaborItem(StyleLaborTableItem sl)
        {
            //db.LaborTable.Remove(sl.LaborItem);
            db.StyleLaborItems.Remove(sl);
        }

        void AddMisc(MiscComponent m, StyleViewModel svm, int keyVal)
        {
            Misc misc = new Misc(m);
            misc.Id = m.Id == 0 ? keyVal : m.Id;
            db.Miscs.Add(misc);
            StyleMisc sm = new StyleMisc() { StyleId = svm.Style.Id, MiscId = misc.Id };
            db.StyleMiscs.Add(sm);
        }

        void RemoveMisc(StyleMisc sm)
        {
            db.Miscs.Remove(sm.Misc);
            db.StyleMiscs.Remove(sm);
        }
        private async Task<bool> SaveImageInStorage(OJewelryDB db, StyleViewModel svm, bool bCopy = false)
        {
            if (svm.Style.Image == null && svm.PostedImageFile == null)
            {
                Trace.TraceInformation("Image and postedfile are both blank, returnig.");
                return true;
            }

            string filename;
            string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            string username = User.Identity.GetUserName();
            env = (env == "Production") ? "" : env + "_";
            // Set filename

            if (svm.PostedImageFile != null) // new image
            {
                if (bCopy) // new style
                {
                    filename = env + "StyleImg_" + username + Path.GetExtension(svm.PostedImageFile.FileName);
                }
                else
                {
                    filename = env + "StyleImg_" + svm.CompanyId.ToString() + "_" + svm.Style.Id.ToString() + "_" + Path.GetExtension(svm.PostedImageFile.FileName);
                }
                Trace.TraceInformation("Uploading {0} to {1}", svm.PostedImageFile.FileName, filename);
                svm.Style.Image = await Singletons.azureBlobStorage.Upload(svm.PostedImageFile, filename);
                Trace.TraceInformation("Done uploading, image=[{0}]", svm.Style.Image);
            } else { // same image
                Uri u = new Uri(svm.Style.Image);
                string blobFile = u.Segments.Last();
                if (bCopy) // new style
                {
                    // Copy old image to new
                    filename = env + "StyleImg_" + username + Path.GetExtension(svm.Style.Image);
                    Trace.TraceInformation("Copying {0} to {1} (bCopy=true)", blobFile, filename);
                    svm.Style.Image = await Singletons.azureBlobStorage.Copy(blobFile, filename);
                    Trace.TraceInformation("Done copying, image=[{0}]", svm.Style.Image);
                }
                else
                {
                    filename = env + "StyleImg_" + svm.CompanyId.ToString() + "_" + svm.Style.Id.ToString() + "_" + Path.GetExtension(svm.Style.Image);
                    if (svm.Style.Image != filename)
                    {
                        Trace.TraceInformation("Copying {0} to {1} (bCopy=false)", blobFile, filename);
                        svm.Style.Image = await Singletons.azureBlobStorage.Copy(blobFile, filename);
                        Trace.TraceInformation("Done copying, image=[{0}]", svm.Style.Image);
                    }
                }
                //svm.Style.Image = await Singletons.azureBlobStorage.Upload(svm.Style.Image, filename);
            }
            return true;
        }


        private void RemoveImageFromStorage(string imageName)
        {
            if (imageName == null) return;
            Uri u = new Uri(imageName);
            string blobFile = u.Segments.Last();
            Singletons.azureBlobStorage.Delete(blobFile);
            return;
        }

        public void CheckModelState(string stylenum)
        {
            foreach (string k in ModelState.Keys)
            {
                ModelState st = ModelState[k];
                if (st.Errors.Count > 0)
                {
                    foreach (ModelError er in st.Errors)
                    {
                        Trace.TraceInformation($"Edit Sytle {stylenum}, key: {k}, msg: {er.ErrorMessage}, exception: {er.Exception}");
                    }
                }
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
