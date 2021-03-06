﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OJewelry.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using System.IO.Packaging;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using OJewelry.Classes;
using System.Diagnostics;

namespace OJewelry.Controllers
{
    [Authorize]
    public class CompaniesController : Controller
    {
        private ApplicationDbContext sec = new ApplicationDbContext();
        private OJewelryDB db = new OJewelryDB();

        // GET: Companies
        public ActionResult Index()
        {

            string loggedOnUserName = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.UserName = loggedOnUserName;
            sec = new ApplicationDbContext();
            ApplicationUser user = sec.Users.Where(x => x.UserName == loggedOnUserName).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(db.Companies.OrderBy(c=>c.Name).ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.FindCompany(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            CompanyViewModel cvm = new CompanyViewModel();
            return View(cvm);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompanyViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                string loggedOnUserName = System.Web.HttpContext.Current.User.Identity.Name;
                sec = new ApplicationDbContext();
                ApplicationUser user = sec.Users.Where(x => x.UserName == loggedOnUserName).FirstOrDefault();
                // Get Phone Number
                cvm.Phone = SetFormattedPhone(cvm.Phone);
                cvm.Phone = GetNormalizedPhone(cvm.Phone);
                CompanyUser cu = new CompanyUser()
                {
                    CompanyId = cvm.Id,
                    UserId = user.Id
                };
                db.CompaniesUsers.Add(cu);
                Vendor vendor = new Vendor
                {
                    Name = "",
                    CompanyId = cvm.Id
                };
                Collection collection = new Collection()
                {
                    CompanyId = cvm.Id,
                    Name = "ASSORTMENT",
                    Notes = $"Default collection for {cvm.Name}"
                };
                // db.Vendors.Add(vendor); - not sure why we do this 7/20/19
                db.AddCompany(new Company(cvm));

                foreach (CompanyViewClientModel c in cvm.clients)
                {
                    if (c.Id == 0)
                    {
                        if (c.Name != null)
                        {
                            c.CompanyID = cvm.Id;
                            Client client = new Client(c);
                            client.Phone = GetNormalizedPhone(c.Phone);
                            db.Clients.Add(client);
                        }
                    }
                    else
                    {
                        if (c.Name != null)
                        {
                            db.Entry(c).State = EntityState.Modified;
                        }
                        else
                        {
                            db.Entry(c).State = EntityState.Deleted;
                        }
                    }
                }

                // add location
                Presenter presenter = new Presenter
                {
                    Id = cvm.Id,
                    Name = cvm.Name,
                    ShortName = cvm.Name.Substring(0, 3),
                    Phone = cvm.Phone,
                    Email = cvm.Email
                };
                db.Presenters.Add(presenter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cvm);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.FindCompany(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            CompanyViewModel cvm = new CompanyViewModel()
            {
                Id = company.Id,
                Name = company.Name,
                Phone = company.Phone,
                Email = company.Email,
                StreetAddr = company.StreetAddr,
                Addr2 = company.Addr2,
                defaultStoneVendor = company.defaultStoneVendor,
                Website = company.Website
            };
            cvm.Id = id.Value;
            //cvm.SetCompany(company);
            cvm.Phone = SetFormattedPhone(company.Phone);

            List<Client> clients = db.Clients.Where(x => x.CompanyID == cvm.Id).ToList();
            foreach(Client c in clients)
            {
                CompanyViewClientModel cvcm =  new CompanyViewClientModel(c);
                cvcm.Phone = SetFormattedPhone(c.Phone);
                cvm.clients.Add(cvcm);
            }
            return View(cvm);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                Company company = db.FindCompany(cvm.Id);
                company.Set(cvm);
                db.Entry(company).State = EntityState.Modified;
                cvm.Phone = GetNormalizedPhone(cvm.Phone);

                foreach (CompanyViewClientModel c in cvm.clients)
                {
                    Client client;
                    client = new Client(c, cvm.Id);
                    client.Phone = GetNormalizedPhone(c.Phone);

                    switch (c.State)
                    {
                        case CVCMState.Added:
                            db.Clients.Add(client);
                            break;
                        case CVCMState.Deleted:
                            db.Entry(client).State = EntityState.Deleted;
                            //db.Clients.Remove(client);
                            break;
                        case CVCMState.Dirty:
                            db.Entry(client).State = EntityState.Modified; 
                            break;
                        case CVCMState.Unadded:
                        case CVCMState.Clean:
                            break;
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cvm);
        }

        private string GetNormalizedPhone(string phone)
        {
            if (phone == null) return phone;
            string [] newPhone = Regex.Split(phone, "[.()-]");
            string finPhone = newPhone[0] + (newPhone.Count() > 1 ? newPhone[1] : "") + (newPhone.Count() > 2 ? newPhone[2] : "");
            return finPhone;
        }

        private string SetFormattedPhone(string phone)
        {
            if (phone == "" || phone == null) return "";
            string newPhone = Regex.Replace(phone, @"^([0-9]{3})([0-9]{3})([0-9]{4})$", @"$1-$2-$3");
            return newPhone;
        }

        // GET: Companies/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.FindCompany(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = CheckSafeDelete(id);

            // Remove collections, locations, clients, components, shapes
            //            List<Collection> collections = db.Collections.Where(col => col.CompanyId == id).ToList();
            //          db.Collections.RemoveRange(collections);
            if (ModelState.IsValid)
            {
                if (company.defaultStoneVendor.HasValue && company.defaultStoneVendor != 0)
                {
                    Vendor defaultStoneVendor = db.Vendors.Find(company.defaultStoneVendor);
                    db.Vendors.Remove(defaultStoneVendor);
                }
                List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == id).ToList();
                db.Vendors.RemoveRange(vendors.ToList());
                db.Collections.RemoveRange(company.Collections.ToList());
                foreach (Presenter p in company.Presenters)
                {
                    db.Contacts.RemoveRange(p.Contacts.ToList());
                }
                db.Presenters.RemoveRange(company.Presenters.ToList());
                db.Clients.RemoveRange(company.Clients.ToList());
                db.Shapes.RemoveRange(company.Shapes.ToList());
                //db.Components.RemoveRange(company.Components);
                db.Stones.RemoveRange(company.Stones.ToList());
                db.Findings.RemoveRange(company.Findings.ToList());
                db.RemoveCompany(company);
                db.SaveChanges();

                return RedirectToAction("Index");
            } else {
                return RedirectToAction("ForcedDelete", new { id = id });
            }
        }

        // GET: Companies/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult ForcedDelete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.FindCompany(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            CheckSafeDelete(id.Value);
            return View(company);
        }

        // POST: Companies/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("ForcedDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult ForcedDeleteConfirmed(int id)
        {
            Company company = db.FindCompany(id);
            // Delete collections
            if (db.Collections.Where(col => col.CompanyId == id && col.Styles.Count() != 0).Count() != 0)
            {
                // Delete Styles
                foreach (Collection col in db.Collections.Where(c => c.CompanyId == id))
                {
                    foreach (Style sty in db.Styles.Where(s => s.CollectionId == col.Id))
                    {
                        // remove components
                        db.Castings.RemoveRange(db.StyleCastings.Where(sc => sc.StyleId == sty.Id).Select(sc => sc.Casting).ToList());
                        db.Labors.RemoveRange(db.StyleLabors.Where(sl => sl.StyleId == sty.Id).Select(sl => sl.Labor).ToList());
                        db.Miscs.RemoveRange(db.StyleMiscs.Where(sm => sm.StyleId == sty.Id).Select(sm => sm.Misc).ToList());
                        // remove links
                        db.StyleCastings.RemoveRange(sty.StyleCastings.ToList());                   
                        db.StyleFindings.RemoveRange(sty.StyleFindings.ToList());
                        db.StyleLaborItems.RemoveRange(sty.StyleLaborItems.ToList());
                        db.StyleLabors.RemoveRange(sty.StyleLabors.ToList());
                        db.StyleMiscs.RemoveRange(sty.StyleMiscs.ToList());
                        db.StyleStones.RemoveRange(sty.StyleStones.ToList());
                        db.Styles.Remove(sty);
                    }
                }
                //return View(company);
            }
            // Delete Jewelry Types
            List<JewelryType> jts = db.JewelryTypes.Where(jt => jt.CompanyId == id).ToList();
            db.JewelryTypes.RemoveRange(jts);
            if (company.defaultStoneVendor.HasValue && company.defaultStoneVendor != 0)
            {
                Vendor defaultStoneVendor = db.Vendors.Find(company.defaultStoneVendor);
                db.Vendors.Remove(defaultStoneVendor);
            }
            List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == id).ToList();
            db.Vendors.RemoveRange(vendors);
            db.Collections.RemoveRange(company.Collections.ToList());
            foreach (Presenter p in company.Presenters)
            {
                db.Contacts.RemoveRange(p.Contacts.ToList());
            }
            db.Presenters.RemoveRange(company.Presenters.ToList());
            db.Clients.RemoveRange(company.Clients.ToList());
            db.Shapes.RemoveRange(company.Shapes.ToList());
            db.Stones.RemoveRange(company.Stones.ToList());
            db.Findings.RemoveRange(company.Findings.ToList());
            db.LaborTable.RemoveRange(company.LaborItems.ToList());
            db.MetalCodes.RemoveRange(db.MetalCodes.Where(mc => mc.CompanyId == id).ToList());
            db.RemoveCompany(company);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        Company CheckSafeDelete(int id)
        {
            Company company = db.FindCompany(id);
            if (db.Collections.Where(col => col.CompanyId == id && col.Styles.Count() != 0).Count() != 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Collection that is not empty ({db.Collections.Where(col => col.CompanyId == id && col.Styles.Count() != 0).Count()}).");
            }
            if (company.Clients.Count > 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Client ({company.Clients.Count}).");
            }
            if (company.Vendors.Count > 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Vendor ({company.Vendors.Count}).");
            }
            if (company.Stones.Count > 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Stone ({company.Stones.Count}).");
            }
            if (company.Findings.Count > 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Finding ({company.Findings.Count}).");
            }
            if (company.Shapes.Count > 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Shape ({company.Shapes.Count}).");
            }
            if (company.Presenters.Count > 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Location ({company.Presenters.Count}).");
            }
            if (db.JewelryTypes.Where(jt => jt.CompanyId == id).Count() != 0)
            {
                ModelState.AddModelError("Company", company.Name + $" has at least one Jewelry Type that is not empty ({db.JewelryTypes.Where(jt => jt.CompanyId == id).Count()}).");
            }
            return company;
        }

        void SaveClients(int companyId, List<CompanyViewClientModel> clients)
        {
            Client cli;
            foreach (CompanyViewClientModel c in clients)
            {
                if (c.Name != null)
                {
                    c.CompanyID = companyId;
                    switch (c.State)
                    {
                        case CVCMState.Added:
                            db.Clients.Add(c.GetClient());
                            break;
                        case CVCMState.Deleted:
                            cli = c.GetClient();
                            db.Entry(cli).State = EntityState.Deleted;
                            db.Clients.Remove(cli);
                            break;
                        case CVCMState.Dirty:
                            cli = c.GetClient();
                            db.Entry(cli).State = EntityState.Modified;
                            break;
                        case CVCMState.Unadded:
                        case CVCMState.Clean:
                        default:
                            break;
                    }
                }
            }
        }

        public ActionResult Inventory(int? CompanyId)
        {
            if (CompanyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (CompanyId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            InventoryViewModel ivm = new InventoryViewModel();
            ivm.CompanyId = CompanyId.Value;
            ivm = SetPresentersLists(ivm);
            return View("Inventory", ivm);
        }

        [HttpPost]
        public ActionResult AddInventory(InventoryViewModel ivm)
        {
            try
            {
                DCTSOpenXML oxl = new DCTSOpenXML();

                String error;
                if (isValidAddModel())//(ModelState.IsValid)
                {
                    using (MemoryStream memstream = new MemoryStream())
                    {
                        ivm.AddPostedFile.InputStream.CopyTo(memstream);
                        Package spreadsheetPackage = Package.Open(memstream, FileMode.Open, FileAccess.Read);
                        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(spreadsheetPackage))
                        {
                            // validate file as spreadsheet
                            WorkbookPart wbPart = spreadsheetDocument.WorkbookPart;
                            Workbook wb = wbPart.Workbook;
                            SharedStringTablePart stringtable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            List<Style> styles = new List<Style>();
                            List<Collection> colls = new List<Collection>();
                            List<Memo> memos = new List<Memo>();
                            Company company = db.FindCompany(ivm.CompanyId);
                            foreach (Sheet sheet in wb.Sheets)
                            {
                                WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                                Worksheet worksheet = wsp.Worksheet;
                                int rc = worksheet.Descendants<Row>().Count();
                                if (rc != 0)
                                {
                                    if (
                                    (oxl.CellMatches("A1", worksheet, stringtable, "Name")) &&
                                    (oxl.CellMatches("B1", worksheet, stringtable, "Jewelry Type")) &&
                                    (oxl.CellMatches("C1", worksheet, stringtable, "Collection")) &&
                                    (oxl.CellMatches("D1", worksheet, stringtable, "Qty")) &&
                                    (oxl.CellMatches("E1", worksheet, stringtable, "Location Short Code")))
                                    {

                                        if (worksheet.Descendants<Row>().Count() >= 2)
                                        {
                                            bool bFoundEmptyRow = false;
                                            int nEmptyRow = -1;
                                            //for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Last().RowIndex; i++, j = i + 1) /* Add checks for empty values */
                                                {
                                                    //process each cell in cols 1-5
                                                    Style style = new Style();
                                                Collection collection = new Collection();
                                                bool bEmptyRow = true;
                                                string invalidCollectionWarning = "";
                                                Cell cell;

                                                // Style Name
                                                style.StyleName = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    style.StyleName = oxl.GetStringVal(cell, stringtable);
                                                }
                                                if (style.StyleName == "")
                                                {
                                                    error = "The style NAME in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
                                                    ModelState.AddModelError("StyleName-" + j, error);
                                                    ivm.Errors.Add(error);
                                                } else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Jewelry Type - find a jewelry type with the same name or reject
                                                string JewelryTypeName = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "B" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    JewelryTypeName = oxl.GetStringVal(cell, stringtable);
                                                }
                                                int JewelryTypeId = GetJewelryTypeId(ivm.CompanyId, JewelryTypeName);
                                                if (JewelryTypeName != "")
                                                {
                                                    bEmptyRow = false;
                                                }
                                                if (JewelryTypeId == -1)
                                                {
                                                    error = "The Jewelry Type [" + JewelryTypeName + "] in sheet [" + sheet.Name + "] row [" + j + "] does not exist.";
                                                    ModelState.AddModelError("JewelryType-" + j, error);
                                                    ivm.Errors.Add(error);
                                                }
                                                else
                                                {
                                                    style.JewelryTypeId = JewelryTypeId;
                                                }

                                                // Collection - find a collection with the same name in this company or reject (ie this is not a means for collection creation)
                                                string CollectionName = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "C" + j.ToString()).FirstOrDefault();
                                                CollectionName = oxl.GetStringVal(cell, stringtable);
                                                int CollectionId = GetCollectionId(CollectionName, company.Id);
                                                if (CollectionName != "")
                                                {
                                                    bEmptyRow = false;
                                                }
                                                if (CollectionId == -1)
                                                {
                                                    // add this row of this sheet to error list
                                                    invalidCollectionWarning = "The Collection [" + CollectionName + "] in sheet [" + sheet.Name + "] row [" + j + "] is blank or does not exist.";
                                                    error = invalidCollectionWarning;
                                                    ModelState.AddModelError("Collection-" + j, error);
                                                    ivm.Errors.Add(error);
                                                    //ivm.CompanyName = db.FindCompany(ivm.CompanyId).Name;
                                                   // style.CollectionId = CreateCompanyCollection(ivm, colls);
                                                }
                                                else
                                                {
                                                    style.CollectionId = CollectionId;
                                                }

                                                // Quantity 
                                                double quantity = 0;
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "D" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    quantity = oxl.GetDoubleVal(cell);
                                                }
                                                if (quantity == 0)
                                                {
                                                    error = "Invalid Quantity in row " + j + " of sheet [" + sheet.Name + "].";
                                                    ModelState.AddModelError("Quantity-" + j, error);
                                                    ivm.Errors.Add(error);
                                                } else {                                                     
                                                    // Quality check for quantity
                                                    bEmptyRow = false;
                                                }

                                                // Location
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "E" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    string presenter = oxl.GetStringVal(cell, stringtable);
                                                    if (presenter == "")
                                                    {
                                                        // add to QOH
                                                        // flag as error
                                                        error = "Location in row " + j + " of sheet [" + sheet.Name + "] is blank.";
                                                        ModelState.AddModelError("Location-" + j, error);
                                                        ivm.Errors.Add(error);
                                                        //style.Quantity = quantity;
                                                    }
                                                    else
                                                    {
                                                        bEmptyRow = false;
                                                        // Is the Presenter in the sheet one of the companies presenters?
                                                        Presenter companyPresenter = company.Presenters.Where(p => p.ShortName?.Trim() == presenter).SingleOrDefault();
                                                        if (companyPresenter != null)
                                                        {
                                                            Memo memo = new Memo()
                                                            {
                                                                PresenterID = companyPresenter.Id,
                                                                StyleID = style.Id,
                                                                Date = DateTime.Now,
                                                                Quantity = quantity,
                                                            };
                                                            style.Memos.Add(memo);
                                                        }
                                                        else
                                                        {
                                                            // flag as error
                                                            error = "Location [" + presenter + "] in row " + j + " of sheet [" + sheet.Name + "] not for " + company.Name + ".";
                                                            ModelState.AddModelError("Location-" + j, error);
                                                            ivm.Errors.Add(error);
                                                        }
                                                    }
                                                } else {
                                                    // flag as error
                                                    error = "Location in row " + j + " of sheet [" + sheet.Name + "] is blank.";
                                                    ModelState.AddModelError("Location-" + j, error);
                                                    ivm.Errors.Add(error);
                                                }

                                                if (bEmptyRow) //Row XX is blank - Any data below Row XX will NOT be updated
                                                {
                                                    nEmptyRow = j;
                                                    bFoundEmptyRow = true;
                                                    //warning = $"Row {j} is blank - Any data below Row {j} will NOT be updated";
                                                    //ivm.Warnings.Add(warning);
                                                    
                                                    if (ModelState.Remove("StyleName-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 5);
                                                    if (ModelState.Remove("JewelryType-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 4);
                                                    if (ModelState.Remove("Collection-" + j)) ivm.Errors.Remove(invalidCollectionWarning);
                                                    if (ModelState.Remove("Quantity-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 2);
                                                    if (ModelState.Remove("Location-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 1);
                                                    // break; // don't any process more of the sheet
                                                }
                                                else
                                                {
                                                    // if there has been an empty row but now there is a row with data, raise the blank row error and stop processing
                                                    if (bFoundEmptyRow)
                                                    {
                                                        error = $"Row {nEmptyRow} is blank";
                                                        ivm.Errors.Add(error);
                                                        break;
                                                    }
                                                    if (ModelState.Where(e => e.Key.Contains($"-{j}")).Count() == 0)
                                                    {
                                                        styles.Add(style);
                                                    }
                                                    //Trace.TraceInformation($"Added row {j} [{style.StyleName}]");
                                                }
                                            }
                                        }
                                        else
                                        { // row count < 2
                                            error = "The spreadsheet [" + sheet.Name + $"] in file [{ivm.AddPostedFile.FileName}] is formatted correctly, but does not contain any data.\n";
                                            ModelState.AddModelError("AddPostedFile-No Rows", error);
                                            ivm.Errors.Add(error);
                                        }
                                    }
                                    else
                                    { // incorrect headers
                                        error = $"The sheet [{sheet.Name}] in file [{ivm.AddPostedFile.FileName}] does not have the correct headers. Please use the New Inventory Template";
                                        ModelState.AddModelError("AddPostedFile-No Headers", error);
                                        ivm.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    // empty sheet
                                    error = $"The sheet [{sheet.Name}] in file [{ivm.AddPostedFile}] is empty. Please use the 'New Inventory' Template";
                                    ModelState.AddModelError("AddPostedFile-Empty Sheet", error);
                                    ivm.Errors.Add(error);
                                }
                                // process collections and styles
                                // new collection; should be exactly 0 or 1 (for company named collection)
                                if (colls.Count != 0 && colls.Count != 1)
                                {
                                    error = "Interal error managing new collections. count = [" + colls.Count + "].";
                                    ivm.Errors.Add(error);
                                }
                                int NewCollectionId = -1;
                                foreach (Collection c in colls)  
                                {
                                    if (db.Collections.Where(x => x.CompanyId == ivm.CompanyId && x.Name == c.Name).Count() == 0)
                                    {
                                        db.Collections.Add(c);
                                    }
                                    NewCollectionId = c.Id;
                                }
                                // existing style - if style already exists, just update the quant
                                Trace.TraceInformation($"Styles found in sheet {styles.Count}");
                                foreach (Style s in styles)
                                {
                                    Trace.TraceInformation($"Starting...");
                                    Collection c;
                                    if (s.CollectionId == -1) {
                                        s.CollectionId = NewCollectionId;
                                    }
                                    c = db.Collections.Find(s.CollectionId);
                                    string cid = "null";
                                    if (c != null) cid = (c.Id).ToString();
                                    Trace.TraceInformation($"StyleName/collection Id: {s.ToString()}{s.StyleName}/[{cid}]");

                                    // nameMatchesInSameCompany
                                    Style dbStyle = db.Styles.Include(dbs => dbs.JewelryType).
                                        Include(dbs => dbs.Collection).
                                        Where(dbs => dbs.StyleName == s.StyleName && dbs.Collection.CompanyId == c.CompanyId).FirstOrDefault();
                                    Trace.TraceInformation($"1");
                                    if (dbStyle != null)
                                    {
                                        Trace.TraceInformation($"2");
                                        // do other relevant fields match
                                        if (dbStyle.JewelryTypeId == s.JewelryTypeId && dbStyle.CollectionId == s.CollectionId)
                                        {
                                            Trace.TraceInformation($"3");
                                            dbStyle.Quantity += s.Quantity;
                                            foreach (Memo m in s.Memos)
                                            {
                                                Trace.TraceInformation($"4");
                                                Memo oldMemo = db.Memos.Where(x => x.PresenterID == m.PresenterID && x.StyleID == dbStyle.Id).SingleOrDefault();
                                                if (oldMemo == null)
                                                {
                                                    dbStyle.Memos.Add(m);
                                                }
                                                else
                                                {
                                                    oldMemo.Quantity += m.Quantity;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Trace.TraceInformation($"5");
                                            // error
                                            string JewelryTypeName = db.JewelryTypes.Find(s.JewelryTypeId).Name;
                                            error = $"Style mismatch in sheet: " +
                                                $"Style Name: {s.StyleName}, Jewelry Type: {JewelryTypeName}, Collection: {c.Name} " +
                                                $"conflicts with saved style {dbStyle.StyleName}/{dbStyle.JewelryType.Name}/{dbStyle.Collection.Name}";
                                            ModelState.AddModelError("", error);
                                            ivm.Errors.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Trace.TraceInformation($"6");
                                        // No style matching name for company, so add it
                                        db.Styles.Add(s);
                                    }
                                }
                                // Done processing, update db
                                if (ivm.Errors.Count() == 0)
                                {
                                    db.SaveChanges();
                                    ViewBag.Message += ivm.AddPostedFile.FileName + " added!";
                                }
                            }
                        }
                    }
                    //blockBlob.DeleteIfExists();
                }
            } catch (Exception e) {
                ViewBag.Message += ("Error processing [" + ivm.AddPostedFile.FileName + "].     ");
                ViewBag.Message += ("Exception[" +e.ToString()+ "] processing [" + ivm.AddPostedFile + "]");
                Trace.TraceError($"AddInv: exception: {e.Message}, [{e.StackTrace}]");
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    Trace.TraceError($"AddInv: Inner exception: {e.Message}, [{e.StackTrace}]");
                }
            } finally {
                ivm = SetPresentersLists(ivm);
            }
            return View("Inventory", ivm);
        }

        [HttpPost]
        public ActionResult MoveInventory(InventoryViewModel ivm)
        {
            try
            {
                String error;
                DCTSOpenXML oxl = new DCTSOpenXML();

                if (isValidMoveModel())//(ModelState.IsValid)
                {
                    // if from == to, error
                    if (ivm.FromLocationId == ivm.ToLocationId)
                    {
                        error = "You cannot from/to the same location";
                        ivm.Errors.Add(error);
                    }

                    // open file
                    using (MemoryStream memstream = new MemoryStream())
                    {
                        ivm.MovePostedFile.InputStream.CopyTo(memstream);
                        Package spreadsheetPackage = Package.Open(memstream, FileMode.Open, FileAccess.Read);

                        // Open a SpreadsheetDocument based on a package.
                        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(spreadsheetPackage))
                        {
                            // validate file as spreadsheet
                            WorkbookPart wbPart = spreadsheetDocument.WorkbookPart;
                            Workbook wb = wbPart.Workbook;
                            SharedStringTablePart stringtable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            List<Style> styles = new List<Style>();
                            List<Collection> colls = new List<Collection>();

                            foreach (Sheet sheet in wb.Sheets)
                            {
                                WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                                Worksheet worksheet = wsp.Worksheet;
                                int rc = worksheet.Descendants<Row>().Count();
                                if (rc != 0)
                                {
                                    int q = worksheet.Descendants<Column>().Count();
                                    if ((true) && //worksheet.Descendants<Column>().Count() >= 4) &&
                                        (oxl.CellMatches("A1", worksheet, stringtable, "Name") &&
                                        oxl.CellMatches("B1", worksheet, stringtable, "Qty")))
                                    {
                                        if (worksheet.Descendants<Row>().Count() >= 2)
                                        {
                                            bool bFoundEmptyRow = false;
                                            int nEmptyRow = -1;
                                            //for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Last().RowIndex; i++, j = i + 1) /* Add checks for empty values */
                                            {
                                                //process each cell in cols 1-4
                                                Style style = new Style();
                                                bool bEmptyRow = true;
                                                //StyleName
                                                style.StyleName = "";
                                                Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null) style.StyleName = oxl.GetStringVal(cell, stringtable);
                                                if (style.StyleName == "")
                                                {
                                                    // error
                                                    error = "The style name in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
                                                    ModelState.AddModelError("StyleNum-"+j, error);
                                                    ivm.Errors.Add(error);
                                                } else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Quantity 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "B" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    style.Quantity = oxl.GetDoubleVal(cell);
                                                    if (style.Quantity == 0)
                                                    {
                                                        error = "The Quantity in sheet [" + sheet.Name + "] row [" + j + "] is blank or 0.";
                                                        ModelState.AddModelError("Quantity-" + j, error);
                                                        ivm.Errors.Add(error);
                                                        bEmptyRow = true;
                                                    } else
                                                    {
                                                        bEmptyRow = false;
                                                    }
                                                }
                                                else
                                                {
                                                    // error
                                                    error = "The Quantity in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
                                                    ModelState.AddModelError("Quantity-" + j, error);
                                                    ivm.Errors.Add(error);
                                                }
                                                if (bEmptyRow)
                                                {
                                                    nEmptyRow = j;
                                                    bFoundEmptyRow = true;
                                                    // Remove last two Model Errors
                                                    if (ModelState.Remove("StyleNum-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 2);
                                                    if (ModelState.Remove("Quantity-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 1);
                                                    //break;
                                                }
                                                else {
                                                    // if there has been an empty row but now there is a row with data, raise the blank row error and stop processing
                                                    if (bFoundEmptyRow)
                                                    {
                                                        error = $"Row {nEmptyRow} is blank";
                                                        ivm.Errors.Add(error);
                                                        break;
                                                    }
                                                    styles.Add(style);
                                                }
                                            }
                                        }
                                        else
                                        { // row count < 2
                                            error = "The spreadsheet [" + sheet.Name + "] is formatted correctly, but does not contain any data.\n";
                                            ivm.Errors.Add(error);
                                            ModelState.AddModelError("MovePostedFile-No Rows", error);
                                        }
                                    }
                                    else
                                    { // incorrect headers
                                        error = "The sheet [" + sheet.Name + "] does not have the correct headers. Please use the Move Inventory Template";
                                        ivm.Errors.Add(error);
                                        ModelState.AddModelError("MovePostedFile-No Headers", error);
                                    }
                                }
                                else
                                {
                                    // empty sheet
                                    error = "The sheet [" + sheet.Name + "] is empty. Please use the 'Move Inventory' Template";
                                    ivm.Errors.Add(error);
                                    ModelState.AddModelError("MovePostedFile-Empty Sheet", error);
                                }
                            }

                            // process moves
                            foreach (Style s in styles)
                            {
                                //List<Collection> colist = db.Collections.Where(x => x.CompanyId == ivm.CompanyId).Include("Styles").ToList();
                                Style theStyle = db.Styles.Include("Collection").Include("JewelryType").Where(x => x.StyleName == s.StyleName).Where(y => y.Collection.CompanyId == ivm.CompanyId).SingleOrDefault();

                                if (theStyle == null)
                                {
                                    error = "The style [" + s.StyleName + "] is not on record.";
                                    ivm.Errors.Add(error);
                                    continue;
                                }
                                // update desc and retail if blank in DB
                                if (theStyle.Desc == null || theStyle.Desc == "") theStyle.Desc = s.Desc;

                                double moveQty = s.Quantity;
                                if (!isValidQuantity(theStyle.JewelryType.Name, moveQty))
                                {
                                    error = "Invalid Quantity: Quantity must be whole number or half number for Earrrings";
                                    ivm.Errors.Add(error);
                                    continue;
                                }
                                // from -= qty, to += qty
                                Memo fromMemo, toMemo;
                                // if moving from home, create a memo. If moving to home, decrease memo qty/remove the memo
                                fromMemo = db.Memos.Where(x => x.PresenterID == ivm.FromLocationId && x.StyleID == theStyle.Id).SingleOrDefault();
                                if ((fromMemo != null) && (moveQty <= fromMemo.Quantity))
                                {
                                    fromMemo.Quantity -= moveQty;
                                    if (fromMemo.Quantity == 0)
                                    {
                                        db.Memos.Remove(fromMemo);
                                    }
                                }
                                else
                                {
                                    Presenter f = db.Presenters.Find(ivm.FromLocationId);
                                    error = "Too few items (" + (fromMemo == null ? 0 : fromMemo.Quantity) + ") in style '" + s.StyleName + "' at " + f.Name + " - cannot move " + moveQty + ".";
                                    ivm.Errors.Add(error);
                                    continue;
                                }
                                // if moving to home
                                //if (ivm.ToLocationId == 0)
                                if (ivm.ToLocationId == -1)
                                {
                                    // if moving to sold
                                    SalesLedger sl = new SalesLedger()
                                    {
                                        StyleId = theStyle.Id,
                                        Date = DateTime.Now,
                                        UnitsSold = moveQty,
                                        StyleInfo = theStyle.RenderInfo()
                                    };
                                    theStyle.UnitsSold += moveQty;
                                    db.SalesLedgers.Add(sl);
                                    // make a new ledger entry
                                }
                                else
                                {
                                    // if moving to other location
                                    toMemo = db.Memos.Where(x => x.PresenterID == ivm.ToLocationId && x.StyleID == theStyle.Id).SingleOrDefault();
                                    if (toMemo == null)
                                    {
                                        toMemo = new Memo()
                                        {
                                            PresenterID = ivm.ToLocationId,
                                            StyleID = theStyle.Id,
                                            Date = DateTime.Now,
                                            Quantity = moveQty,
                                            Notes = ""
                                        };
                                        db.Memos.Add(toMemo);
                                    }
                                    else
                                    {
                                        toMemo.Quantity += moveQty;
                                    }
                                }
                            }
                            // Done processing, update db
                            if (ivm.Errors.Count() == 0)
                            {
                                db.SaveChanges();
                                ViewBag.Message += ivm.MovePostedFile.FileName + " added!";

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Message += ("Exception[" + e.ToString() + "]");
                Trace.TraceError($"OJException: in MoveInventory: {e.Message}");
            }
            finally
            {
                ivm = SetPresentersLists(ivm);
            }
            return View("Inventory", ivm);
        }

        public ActionResult InventoryReport(int? CompanyId)
        {
            if (CompanyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (CompanyId == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            InventoryReportModel irm = SetIRM(CompanyId.Value);
            return View(irm);
        }

        public FileResult SaveNewStyleTemplate()
        {
            return File("~/Excel/NewInv.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult SaveMoveStyleTemplate()
        {
            return File("~/Excel/MoveInv.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult SaveStoneInventoryTemplate()
        {
            return File("~/Excel/StoneInv.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult SaveFindingInventoryTemplate()
        {
            return File("~/Excel/FindingInv.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public FileResult ExportInventoryReport(int? CompanyId, string sCurrDate)
        {
            if (CompanyId == null)
            {
                //return RedirectToAction("Index", "Home");
            }
            if (CompanyId == 0)
            {
                //return RedirectToAction("Index", "Home");
            }
            InventoryReportModel irm = SetIRM(CompanyId.Value);
            byte[] b;

            DateTime curr;
            sCurrDate = sCurrDate.Replace("'", "");
            if (!DateTime.TryParse(sCurrDate, out curr))
            {
                curr = DateTime.Now.ToLocalTime();
            }
            string currDate = $"{curr.ToShortDateString()} {curr.ToShortTimeString()}";

            DCTSOpenXML oxl = new DCTSOpenXML();
            using (MemoryStream memStream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(memStream, SpreadsheetDocumentType.Workbook))
                {
                    // Build Excel File
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
//                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = irm.CompanyName + " Inventory" // as of " + dt.ToShortDateString()
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();

                    // declare locals
                    Row row;
                    Cell cell;
                    char ch;
                    string loc;
                    int rr;

                    // Date row
                    row = new Row();
                    cell = oxl.SetCellVal("A1", $"Export - Inventory  {currDate}");
                    row.Append(cell);
                    sd.Append(row);

                    row = new Row();
                    cell = oxl.SetCellVal("A2", "");
                    row.Append(cell);
                    sd.Append(row);

                    // Header row
                    // Save Col A for image
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A3", "Style"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B3", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C3", "Description"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D3", "Jewelry Type"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 5, Max = 5, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E3", "Collection"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 6, Max = 6, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F3", "Retail"); row.Append(cell);
                    //cell = oxl.SetCellVal("G2", irm.CompanyName); row.Append(cell);
                    //cell = oxl.SetCellVal("G2", "QOH"); row.Append(cell);
                    for (int i = 0; i < irm.locations.Count(); i++)
                    {
                        ch = (char)(((int)'G') + i);
                        loc = ch + "3";
                        oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = (uint)i + 7, Max = (uint)i + 7, BestFit = true, CustomWidth = true });
                        cell = oxl.SetCellVal(loc, irm.locations[i].ShortName);
                        row.Append(cell);
                    }
                    /* Ignore Sold
                    ch = (char)(((int)'G') + irm.locations.Count());
                    loc = ch + "2";
                    cell = oxl.SetCellVal(loc, "SOLD");
                    */
                    //row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);

                    // Loop thru styles
                    for (int i = 0; i < irm.styles.Count; i++)
                    {
                        row = new Row();
                        rr = 4 + i;
                        loc = "A" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].StyleNum); row.Append(cell);
                        loc = "B" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].StyleName); row.Append(cell);
                        loc = "C" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].StyleDesc); row.Append(cell);
                        loc = "D" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].JewelryTypeName); row.Append(cell);
                        loc = "E" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].StyleCollectionName); row.Append(cell);
                        loc = "F" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].StylePrice); row.Append(cell);
                        //loc = "G" + rr.ToString(); cell = oxl.SetCellVal(loc, irm.styles[i].StyleQuantity); row.Append(cell);

                        for (int j = 0; j < irm.locations.Count; j++)
                        {
                            string jewelrytype = irm.styles[i].JewelryTypeName.ToLower();
                            ch = (char)(((int)'G') + j);
                            loc = ch.ToString() + rr.ToString();
                            irmLS irmls = irm.locationQuantsbystyle.
                                Where(x => x.StyleId == irm.styles[i].StyleId && x.PresenterId == irm.locations[j].PresenterId).SingleOrDefault();
                            if (irmls != null)
                            {
                                double q = irmls.MemoQty;
                                cell = oxl.SetCellVal(loc, irmls.MemoQty);
                            }
                            else
                            {
                                cell = oxl.SetCellVal(loc, 0);
                            }
                            row.Append(cell);
                           
                        }
                        /* Ignore SOLD
                        ch = (char)(((int)'G') + irm.locations.Count());
                        loc = ch.ToString() + rr.ToString();
                        cell = oxl.SetCellVal(loc, irm.styles[i].StyleQtySold); row.Append(cell);
                        */
                        sd.Append(row);
                     }
                     worksheet.Append(sd);
                     worksheetPart.Worksheet = worksheet;
                     workbookPart.Workbook.Save();
                     document.Close();
                }
                b = memStream.ToArray();
                return File(b, 
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                    irm.CompanyName + " Inventory as of " + currDate + ".xlsx");
            }
        }

        InventoryReportModel SetIRM(int CompanyId)
        {
            InventoryReportModel irm = new InventoryReportModel();
            irm.styles = db.Styles.
                Join(db.Collections,
                x => x.CollectionId,
                cl => cl.Id,
                (x, cl) => new
                {
                    StyleId = x.Id,
                    //StyleNum = x.StyleNum,
                    StyleQuantity = x.Quantity,
                    StyleName = x.StyleName,
                    StyleDesc = x.Desc,
                    StylePrice = x.RetailPrice,
                    StyleSold = x.UnitsSold,
                    JewelryTypeName = x.JewelryType.Name,
                    x.CollectionId,
                    cl.CompanyId,
                    cl.Name,
                }).
                    Where(x => x.CompanyId == CompanyId).
                    Join(db.Companies,
                x => x.CompanyId,
                cp => cp.Id,
                (x, cp) => new irmStyle()
                {
                    StyleId = x.StyleId,
                    //StyleNum = x.StyleNum,
                    StyleQuantity = x.StyleQuantity,
                    StyleName = x.StyleName,
                    StyleDesc = x.StyleDesc,
                    StylePrice = x.StylePrice ?? 0,
                    StyleQtySold = x.StyleSold,
                    JewelryTypeName = x.JewelryTypeName,
                    StyleCollectionName = x.Name
                }).Distinct().ToList();

            // irmStyle() { StyleId = s.Id, StyleNum = s.StyleNum, StyleQuantity = s.Quantity, StyleName = s.StyleName, StyleDesc = s.Desc, s.CollectionId }
            /*
            irm.locations = db.Styles.Join(
                db.Memos,
                s => s.Id,
                m => m.StyleID,
                (s, m) => new { m.PresenterID }).Join(
                db.Presenters,
                x => x.PresenterID,
                p => p.Id,
                (x, p) => new { PresenterId = p.Id, PresenterName = p.Name, p.CompanyId, p.ShortName }).Where(x => x.CompanyId == CompanyId)
                .Where(x => x.CompanyId == CompanyId).Join(
                db.Companies,
                x => x.CompanyId,
                c => c.Id,
                (x, c) => new irmLocation() { PresenterId = x.PresenterId, PresenterName = x.PresenterName, ShortName = x.ShortName }).Distinct().ToList();
            */
            irm.locations = db.Presenters.Where(p => p.CompanyId == CompanyId).Select(p => new irmLocation()
                { PresenterId = p.Id, PresenterName = p.Name, ShortName = p.ShortName }).ToList();
            irm.locationQuantsbystyle = db.Styles.Join(
                db.Memos,
                s => s.Id,
                m => m.StyleID,
                (s, m) => new { m.PresenterID, m.Quantity, s.Id, s.StyleName, sQty = s.Quantity }).Join(
                db.Presenters,
                x => x.PresenterID,
                p => p.Id,
                (x, p) => new
                {
                    StyleId = x.Id,
                    PresenterId = p.Id,
                    StyleName = x.StyleName,
                    LocationName = p.Name,
                    MemoQty = x.Quantity,
                    StyleQuantity = x.sQty,
                    p.CompanyId
                }).Where(x => x.CompanyId == CompanyId).Where(x => x.CompanyId == CompanyId).Join(
                db.Companies,
                x => x.CompanyId,
                c => c.Id,
                (x, c) => new irmLS()
                {
                    StyleId = x.StyleId,
                    PresenterId = x.PresenterId,
                    StyleName = x.StyleName,
                    LocationName = x.LocationName,
                    MemoQty = x.MemoQty,
                    StyleQuantity = x.StyleQuantity
                }).
                Distinct().OrderBy(x => x.LocationName).OrderBy(x => x.StyleName).ToList();
            irm.CompanyId = CompanyId;
            irm.CompanyName = db.FindCompany(CompanyId).Name;

            return irm;
        }

        public ActionResult ManageStoneInventory(int companyId)
        {
            StoneInventoryModel sim = new StoneInventoryModel()
            {
                CompanyId = companyId
            };
            
            sim.CompanyName = db.FindCompany(companyId)?.Name;
            return View(sim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageStoneInventory(StoneInventoryModel sim)
        {
            /*
             * Open the sheet, check for format of headers. 
             * Then, for each row, find its stone and update the qty with delta.
             * If item is not found add it to the warnings.
            */
            try
            {
                DCTSOpenXML oxl = new DCTSOpenXML();

                String error;
                if (isValidStoneModel())
                {
                    using (MemoryStream memstream = new MemoryStream())
                    {
                        sim.PostedFile.InputStream.CopyTo(memstream);
                        sim.failedFileName = sim.PostedFile.FileName;
                        Package spreadsheetPackage = Package.Open(memstream, FileMode.Open, FileAccess.Read);
                        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(spreadsheetPackage))
                        {
                            // validate file as spreadsheet
                            WorkbookPart wbPart = spreadsheetDocument.WorkbookPart;
                            Workbook wb = wbPart.Workbook;
                            SharedStringTablePart stringtable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            List<Style> styles = new List<Style>();
                            List<Collection> colls = new List<Collection>();
                            List<StoneElement> stoneElements = new List<StoneElement>();

                            foreach (Sheet sheet in wb.Sheets)
                            {
                                WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                                Worksheet worksheet = wsp.Worksheet;
                                int rc = worksheet.Descendants<Row>().Count();
                                if (rc != 0)
                                {
                                    int q = worksheet.Descendants<Column>().Count();
                                    if ((true) && //worksheet.Descendants<Column>().Count() >= 4) &&
                                        (oxl.CellMatches("A1", worksheet, stringtable, "Stone") &&
                                        oxl.CellMatches("B1", worksheet, stringtable, "Shape") &&
                                        oxl.CellMatches("C1", worksheet, stringtable, "Size") &&
                                        oxl.CellMatches("D1", worksheet, stringtable, "Vendor") &&
                                        oxl.CellMatches("E1", worksheet, stringtable, "Qty")))
                                    {
                                        if (worksheet.Descendants<Row>().Count() >= 2)
                                        {
                                            bool bFoundEmptyRow = false;
                                            int nEmptyRow = -1;
                                            int count = worksheet.Descendants<Row>().Count();
                                            Cell LastCell = worksheet.Descendants<Cell>().Last();
                                            uint LastRow = worksheet.Descendants<Row>().Last().RowIndex;

                                            //for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Last().RowIndex; i++, j = i + 1) /* Add checks for empty values */
                                            {
                                                // Read Stone, Size, Shape                                                
                                                string stone = "", shape="", size ="", vendor = "";
                                                int delta = 0;
                                                //process each cell in cols 1-5
                                                bool bEmptyRow = true;
                                                // Stone
                                                Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null) stone = oxl.GetStringVal(cell, stringtable);
                                                if (stone == "")
                                                {
                                                    // error
                                                    error = "The stone in sheet [" + sheet.Name + "] cell [A" + j + "] is blank.";
                                                    ModelState.AddModelError("Stone-" + j, error);
                                                    sim.Errors.Add(error);
                                                }
                                                else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Shape 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "B" + j.ToString()).FirstOrDefault();
                                                if (cell != null) shape = oxl.GetStringVal(cell, stringtable);
                                                if (shape == "")
                                                {
                                                    // error
                                                    error = "The shape in sheet [" + sheet.Name + "] cell [B" + j + "] is blank.";
                                                    ModelState.AddModelError("Shape-" + j, error);
                                                    sim.Errors.Add(error);
                                                }
                                                else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Size 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "C" + j.ToString()).FirstOrDefault();
                                                if (cell != null) size = oxl.GetStringVal(cell, stringtable);
                                                if (size == "")
                                                {
                                                    // error
                                                    error = "The size in sheet [" + sheet.Name + "] cell [C" + j + "] is blank.";
                                                    ModelState.AddModelError("Size-" + j, error);
                                                    sim.Errors.Add(error); 
                                                }
                                                else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Vendor 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "D" + j.ToString()).FirstOrDefault();
                                                if (cell != null) vendor = oxl.GetStringVal(cell, stringtable);
                                                if (vendor == "")
                                                {
                                                    // error
                                                    error = "The Vendor in sheet [" + sheet.Name + "] cell [D" + j + "] is blank.";
                                                    ModelState.AddModelError("Vendor-" + j, error);
                                                    sim.Errors.Add(error); 
                                                }
                                                else
                                                {
                                                    bEmptyRow = false;
                                                }


                                                // Delta 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "E" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    delta = oxl.GetIntVal(cell);
                                                }
                                                if (delta == 0)
                                                {
                                                    // error
                                                    error = "The Quantity in sheet [" + sheet.Name + "] cell [E" + j + "] is 0 or blank";
                                                    ModelState.AddModelError("Qty-" + j, error);
                                                    sim.Errors.Add(error);
                                                } else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // if whole row is blank, remove errors and flag as warning, don't add the style.
                                                if (bEmptyRow)
                                                {
                                                    nEmptyRow = j;
                                                    bFoundEmptyRow = true;
                                                    // Remove last two Model Errors, add warning
                                                    //string warning = $"Row {j} is blank - Any data below Row {j} will NOT be updated";
                                                    //sim.Warnings.Add(warning);
                                                    string s = sim.Errors.Find(x => x == "Stone-" + j);
                                                    if (ModelState.Remove("Stone-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 5);
                                                    if (ModelState.Remove("Shape-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 4);
                                                    if (ModelState.Remove("Size-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 3);
                                                    if (ModelState.Remove("Vendor-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 2);
                                                    if (ModelState.Remove("Qty-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 1);
                                                    //break;
                                                }
                                                else
                                                {
                                                    // if there has been an empty row but now there is a row with data, raise the blank row error and stop processing
                                                    if (bFoundEmptyRow)
                                                    {
                                                        error = $"Row {nEmptyRow} is blank";
                                                        sim.Errors.Add(error);
                                                        break;
                                                    }
                                                    // add to list
                                                    stoneElements.Add(new StoneElement()
                                                    {
                                                        stone = stone,
                                                        shape = shape,
                                                        size = size,
                                                        vendorName = vendor,
                                                        delta = delta,
                                                        lineNum = j
                                                    });
                                                }
                                            }
                                        }
                                        else
                                        { // row count < 2
                                            error = "The spreadsheet [" + sheet.Name + "] is formatted correctly, but does not contain any data.\n";
                                            sim.Errors.Add(error);
                                            ModelState.AddModelError("StoneInv-No Rows", error);
                                        }
                                    }
                                    else
                                    { // incorrect headers
                                        error = "The sheet [" + sheet.Name + "] does not have the correct headers. Please use the 'Stone Inventory' Template";
                                        sim.Errors.Add(error);
                                        ModelState.AddModelError("StoneInv-No Headers", error);
                                    }
                                }
                                else
                                {
                                    // empty sheet
                                    error = "The sheet [" + sheet.Name + "] is empty. Please use the 'Stone Inventory' Template";
                                    sim.Errors.Add(error);
                                    ModelState.AddModelError("StoneInv-Empty Sheet", error);
                                }
                            }

                            // process deltas
                            foreach (StoneElement s in stoneElements)
                            {
                                Stone theStone = db.Stones.Include("Shape").Include("Vendor")
                                    .Where(x => x.Name.Trim() == s.stone.Trim() && x.Shape.Name.Trim() == s.shape.Trim() && x.StoneSize.Trim() == s.size.Trim() &&
                                        x.CompanyId == sim.CompanyId).Where(x => x.Vendor == null || x.Vendor.Name == null || x.Vendor.Name.Trim() == "" ||
                                        (x.Vendor != null && x.Vendor.Name.Trim() != "" && x.Vendor.Name.Trim() == s.vendorName.Trim()))
                                    .SingleOrDefault();

                                if (theStone == null) 
                                {
                                    if (!(string.IsNullOrEmpty(s.stone) || string.IsNullOrEmpty(s.size) || string.IsNullOrEmpty(s.shape) || string.IsNullOrEmpty(s.vendorName)))
                                    {
                                        error = $"The stone in cells [A{s.lineNum}:E{s.lineNum}] is not on record ([{s.stone}]/[{s.size}]/[{s.shape}]/[{s.vendorName}]).";
                                        sim.Errors.Add(error);
                                    }
                                    continue;
                                }

                                // update quantity
                                if (theStone.Qty + s.delta >= 0)
                                {
                                    theStone.Qty += s.delta;
                                } else {
                                    error = $"Insufficient inventory ({theStone.Qty}) to remove ({s.delta}) stones in cell [E{s.lineNum}]";
                                    sim.Errors.Add(error);
                                }
                            }
                            // Done processing, update db
                            if (sim.Errors.Count() == 0)
                            {
                                db.SaveChanges();
                                ViewBag.Message += sim.PostedFile.FileName + " updated";
                            }
                        }
                    }
                }
            } catch (Exception e) {
                sim.Errors.Add($"Fatal exception:{e.Message}\n{e.StackTrace}");
                sim.Errors.Add($"Fatal exception:{e.InnerException}");
                ModelState.AddModelError("Caught fatal exception", e);
                Trace.TraceError($"OJException: ManageStoneInventory: {e.Message}");
            }
            if (sim.Errors.Count() == 0)
            {
                sim.success = true;
            } else {
                sim.success = false;
            }
            return View(sim);
        }

        public ActionResult ManageFindingsInventory(int companyId)
        {
            FindingInventoryModel fim = new FindingInventoryModel()
            {
                CompanyId = companyId
            }; 
            fim.CompanyName = db.FindCompany(companyId)?.Name;
            return View(fim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageFindingsInventory(FindingInventoryModel fim)
        {
            /*
             * Open the sheet, check for format of headers. 
             * Then, for each row, find its finding and update the qty with delta.
             * If item is not found add it to the warnings.
            */
            try
            {
                DCTSOpenXML oxl = new DCTSOpenXML();

                String error;
                if (isValidFindingModel())
                {
                    using (MemoryStream memstream = new MemoryStream())
                    {
                        fim.PostedFile.InputStream.CopyTo(memstream);
                        fim.failedFileName = fim.PostedFile.FileName;
                        Package spreadsheetPackage = Package.Open(memstream, FileMode.Open, FileAccess.Read);
                        using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(spreadsheetPackage))
                        {
                            // validate file as spreadsheet
                            WorkbookPart wbPart = spreadsheetDocument.WorkbookPart;
                            Workbook wb = wbPart.Workbook;
                            SharedStringTablePart stringtable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                            List<Style> styles = new List<Style>();
                            List<Collection> colls = new List<Collection>();
                            List<FindingElement> findingElements = new List<FindingElement>();

                            foreach (Sheet sheet in wb.Sheets)
                            {
                                WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                                Worksheet worksheet = wsp.Worksheet;
                                int rc = worksheet.Descendants<Row>().Count();
                                if (rc != 0)
                                {
                                    int q = worksheet.Descendants<Column>().Count();
                                    if ((true) && //worksheet.Descendants<Column>().Count() >= 4) &&
                                        (oxl.CellMatches("A1", worksheet, stringtable, "Finding") &&
                                        oxl.CellMatches("B1", worksheet, stringtable, "Vendor") &&
                                        oxl.CellMatches("C1", worksheet, stringtable, "Qty")))
                                    {
                                        if (worksheet.Descendants<Row>().Count() >= 2)
                                        {
                                            bool bFoundEmptyRow = false;
                                            int nEmptyRow = -1;
                                            //for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Last().RowIndex; i++, j = i + 1) /* Add checks for empty values */
                                            {
                                                // Read finding
                                                string finding = "", vendor = "";
                                                int delta = 0;
                                                //process each cell in cols 1-3
                                                bool bEmptyRow = true;
                                                // Stone
                                                Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null) finding = oxl.GetStringVal(cell, stringtable);
                                                if (finding == "")
                                                {
                                                    // error
                                                    error = "The finding in sheet [" + sheet.Name + "] cell [A" + j + "] is blank.";
                                                    ModelState.AddModelError("Finding-" + j, error);
                                                    fim.Errors.Add(error);
                                                }
                                                else
                                                {
                                                    bEmptyRow = false;
                                                }


                                                // Vendor 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "B" + j.ToString()).FirstOrDefault();
                                                if (cell != null) vendor = oxl.GetStringVal(cell, stringtable);
                                                if (vendor == "")
                                                {
                                                    // error
                                                    error = "The Vendor in sheet [" + sheet.Name + "] cell [B" + j + "] is blank.";
                                                    ModelState.AddModelError("Vendor-" + j, error);
                                                    fim.Errors.Add(error);
                                                }
                                                else
                                                {
                                                    bEmptyRow = false;
                                                }


                                                // Delta 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "C" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    delta = oxl.GetIntVal(cell);
                                                }
                                                if (delta == 0)
                                                {
                                                    // error
                                                    error = "The Quantity in sheet [" + sheet.Name + "] cell [C" + j + "] is 0 or blank";
                                                    ModelState.AddModelError("Qty-" + j, error);
                                                    fim.Errors.Add(error);
                                                } else
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // if whole row is blank, remove errors and flag as warning, don't add the style.
                                                if (bEmptyRow)
                                                {
                                                    nEmptyRow = j;
                                                    bFoundEmptyRow = true;
                                                    // Remove last two Model Errors, add warning

                                                    string s = fim.Errors.Find(x => x == "Stone-" + j);
                                                    if (ModelState.Remove("Finding-" + j)) fim.Errors.RemoveAt(fim.Errors.Count - 3);
                                                    if (ModelState.Remove("Vendor-" + j)) fim.Errors.RemoveAt(fim.Errors.Count - 2);
                                                    if (ModelState.Remove("Qty-" + j)) fim.Errors.RemoveAt(fim.Errors.Count - 1);
                                                }
                                                else
                                                {
                                                    // if there has been an empty row but now there is a row with data, raise the blank row error and stop processing
                                                    if (bFoundEmptyRow)
                                                    {
                                                        error = $"Row {nEmptyRow} is blank";
                                                        fim.Errors.Add(error);
                                                        break;
                                                    }
                                                    // add to list
                                                    findingElements.Add(new FindingElement()
                                                    {
                                                        finding = finding,
                                                        vendorName = vendor,
                                                        delta = delta,
                                                        lineNum = j
                                                    });
                                                }
                                            }
                                        }
                                        else
                                        { // row count < 2
                                            error = "The spreadsheet [" + sheet.Name + "] is formatted correctly, but does not contain any data.\n";
                                            fim.Errors.Add(error);
                                            ModelState.AddModelError("FindingInv-No Rows", error);
                                        }
                                    }
                                    else
                                    { // incorrect headers
                                        error = "The sheet [" + sheet.Name + "] does not have the correct headers. Please use the 'Finding Inventory' Template";
                                        fim.Errors.Add(error);
                                        ModelState.AddModelError("FindingInv-No Headers", error);
                                    }
                                }
                                else
                                {
                                    // empty sheet
                                    error = "The sheet [" + sheet.Name + "] is empty. Please use the 'Finding Inventory' Template";
                                    fim.Errors.Add(error);
                                    ModelState.AddModelError("FindingInv-Empty Sheet", error);
                                }
                            }

                            // process deltas

                            foreach (FindingElement f in findingElements)
                            {
                                List<Finding> these = db.Findings.Include("Vendor")
                                    .Where(x => x.Name.Trim() == f.finding.Trim() && x.CompanyId == fim.CompanyId)
                                    .ToList();
                                Finding theFinding = these.Where(x => x.Vendor == null || x.Vendor.Name == null || x.Vendor.Name.Trim() == "" ||
                                        (x.Vendor != null && x.Vendor.Name.Trim() != "" && x.Vendor.Name.Trim() == f.vendorName.Trim()))
                                    .SingleOrDefault();

                                if (theFinding == null)
                                {
                                    error = $"The finding in cells [A{f.lineNum}:C{f.lineNum}] is not on record.";
                                    fim.Errors.Add(error);
                                    continue;
                                }
                                // update quantity
                                if (theFinding.Qty + f.delta >= 0)
                                {
                                    theFinding.Qty += f.delta;
                                }
                                else
                                {
                                    error = $"Insufficient inventory ({theFinding.Qty}) to remove ({f.delta}) finding in cell [C{f.lineNum}]";
                                    fim.Errors.Add(error);
                                }
                            }
                            // Done processing, update db
                            if (fim.Errors.Count() == 0)
                            {
                                db.SaveChanges();
                                ViewBag.Message += fim.PostedFile.FileName + " updated";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                fim.Errors.Add($"Fatal exception:{e.Message}\n{e.StackTrace}");
                fim.Errors.Add($"Fatal exception:{e.InnerException}");
                ModelState.AddModelError("Caught fatal exception", e);
                Trace.TraceError($"OJException: ManageFindingsInventory: {e.Message}");
            }
            if (fim.Errors.Count() == 0)
            {
                fim.success = true;
            }
            else
            {
                fim.success = false;
            }
            return View(fim);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        int GetJewelryTypeId(int companyId, string JewelryTypeName)
        {
            JewelryType jt = db.JewelryTypes.Where(j => j.Name == JewelryTypeName && j.CompanyId == companyId).FirstOrDefault();
            if (jt == null)
            {
                /*
                string s = "The Jewelry Type [" + JewelryTypeName + "] does not exist.";
                ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(s);
                throw ex;
                */
                return -1;
            }
            return jt.Id;
        }

        int GetCollectionId(string CollectionName, int companyId)
        {
            Collection co = db.Collections.Where(c => c.Name == CollectionName && c.CompanyId == companyId).FirstOrDefault();
            if (co == null)
            {
                return -1;
            }
            return co.Id;
        }

        int CreateCompanyCollection(InventoryViewModel ivm, List<Collection> colls)
        {
            int CollectionId = GetCollectionId(ivm.CompanyName, ivm.CompanyId);
            if (CollectionId == -1)
            {
                CollectionId = GetCollectionId("ASSORTMENT", ivm.CompanyId);
            }
            if (CollectionId == -1 && ivm.bCC_CompCollCreated == false)
            {
                // Make new collection
                Collection c = new Collection()
                {
                    CompanyId = ivm.CompanyId,
                    Name = "ASSORTMENT",
                };
                colls.Add(c);
                ivm.bCC_CompCollCreated = true;
            }
            //CollectionId = -1;
            
            return CollectionId;
        }

        InventoryViewModel SetPresentersLists(InventoryViewModel ivm)
        {
            Company co = db.FindCompany(ivm.CompanyId);
            ivm.CompanyName = co.Name;
            List<Presenter> tpl = db.Presenters.Where(x => x.CompanyId == ivm.CompanyId).ToList();
            /* Don't use company name as a target 
            Presenter pSo = new Presenter();
            pSo.Id = 0;
            pSo.Name = ivm.CompanyName;
            tpl.Insert(0, pSo);
            */
            List<Presenter> fpl = tpl.ToList();
            ivm.FromLocations = new SelectList(fpl, "Id", "Name");
            /* Don't Use SOLD
            Presenter pCo = new Presenter();
            pCo.Id = -1;
            pCo.Name = "SOLD";
            tpl.Add(pCo);
            */
            ivm.ToLocations = new SelectList(tpl, "Id", "Name");
            return ivm;
        }

        bool isValidAddModel()
        {
            bool b = true;
            // Iterate thru ModelState: if errors in Add, then there is a problem
            foreach (KeyValuePair<string, ModelState> m in ModelState)
            {
                if (m.Key == "AddPostedFile" && m.Value.Errors.Count != 0)
                {
                    b =  false;
                }
                if (m.Key == "MovePostedFile" && m.Value.Errors.Count != 0)
                {
                    m.Value.Errors.Clear();
                }

            }
            return b;
        }

        bool isValidMoveModel()
        {
            bool b = true;
            // Iterate thru ModelState: if errors in From, To, or Move, then there is a problem
            foreach (KeyValuePair<string, ModelState> m in ModelState)
            {
                if (m.Key == "MovePostedFile" && m.Value.Errors.Count != 0)
                {
                    b = false;
                }
                if (m.Key == "AddPostedFile" && m.Value.Errors.Count != 0)
                {
                    m.Value.Errors.Clear();
                }

            }
            return b;
        }

        bool isValidStoneModel()
        {
            bool b = true;
            // Iterate thru ModelState: if errors in From, To, or Move, then there is a problem
            foreach (KeyValuePair<string, ModelState> m in ModelState)
            {
                if (m.Key == "PostedFile" && m.Value.Errors.Count != 0)
                {
                    b = false;
                }
            }
            return b;
        }

        bool isValidFindingModel()
        {
            bool b = true;
            // Iterate thru ModelState: if errors in From, To, or Move, then there is a problem
            foreach (KeyValuePair<string, ModelState> m in ModelState)
            {
                if (m.Key == "PostedFile" && m.Value.Errors.Count != 0)
                {
                    b = false;
                }
            }
            return b;
        }

        bool isValidQuantity(string jewelryType, double qty)
        {
            if (qty <= 0) return false;
            int i = Convert.ToInt32(Math.Floor(qty));
            if (i == qty) return true;
            if (jewelryType.ToLower() == "earring" || jewelryType.ToLower() == "earrings")
            {
                if (i == qty-0.5) return true;
            }
            return false;
        }
    }
}

