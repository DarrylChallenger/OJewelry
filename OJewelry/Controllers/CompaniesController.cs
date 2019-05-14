using System;
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
                cvm.company.Phone = GetNormalizedPhone(cvm.company.Phone);
                CompanyUser cu = new CompanyUser()
                {
                    CompanyId = cvm.company.Id,
                    UserId = user.Id
                };
                cvm.company.CompanyUsers.Add(cu);
                Vendor vendor = new Vendor
                {
                    Name = "",
                    CompanyId = cvm.company.Id
                };

                db.Vendors.Add(vendor);
                db.AddCompany(cvm.company);

                foreach (CompanyViewClientModel c in cvm.clients)
                {
                    if (c.Id == 0)
                    {
                        if (c.Name != null)
                        {
                            c.CompanyID = cvm.company.Id;
                            Client client = new Client(c);
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
                    Id = cvm.company.Id,
                    Name = cvm.company.Name,
                    ShortName = cvm.company.Name,
                    Phone = cvm.company.Phone,
                    Email = cvm.company.Email
                };
                db.Presenters.Add(presenter);
                db.SaveChanges();

                Company company = db.FindCompany(cvm.company.Id);
                company.defaultStoneVendor = vendor.Id;
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
            CompanyViewModel cvm = new CompanyViewModel();
            cvm.company = company;
            cvm.company.Phone = SetFormattedPhone(company.Phone);

            List<Client> clients = db.Clients.Where(x => x.CompanyID == cvm.company.Id).ToList();
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
                db.Entry(cvm.company).State = EntityState.Modified;
                cvm.company.Phone = GetNormalizedPhone(cvm.company.Phone);

                foreach (CompanyViewClientModel c in cvm.clients)
                {
                    Client client;
                    client = new Client(c, cvm.company.Id);
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
            string finPhone = newPhone[0] + newPhone[1] + newPhone[2];
            return finPhone;
        }

        private string SetFormattedPhone(string phone)
        {
            if (phone == "" || phone == null) return "";
            string newPhone = Regex.Replace(phone, @"^([0-9]{3})([0-9]{3})([0-9]{4})$", @"$1-$2-$3");
            return newPhone;
        }

        // GET: Companies/Delete/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Company company = db.FindCompany(id);
            if (db.Collections.Where(col => col.CompanyId == id && col.Styles.Count() != 0).Count() != 0)
            {
                ModelState.AddModelError("Company", company.Name + " has at least one Collection that is not empty.");
                return View(company);
            }
            if (db.JewelryTypes.Where(jt => jt.CompanyId == id).Count() != 0)
            {
                ModelState.AddModelError("Company", company.Name + " has at least one Jewelry Type that is not empty.");
                return View(company);
            }
            // Remove collections, locations, clients, components
//            List<Collection> collections = db.Collections.Where(col => col.CompanyId == id).ToList();
  //          db.Collections.RemoveRange(collections);
            if (company.defaultStoneVendor.HasValue && company.defaultStoneVendor != 0)
            {
                Vendor defaultStoneVendor = db.Vendors.Find(company.defaultStoneVendor);
                db.Vendors.Remove(defaultStoneVendor);
            }
            List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == id).ToList();
            db.Vendors.RemoveRange(vendors);
            db.Collections.RemoveRange(company.Collections);
            db.Presenters.RemoveRange(company.Presenters);
            db.Clients.RemoveRange(company.Clients);
            //db.Components.RemoveRange(company.Components);
            db.Stones.RemoveRange(company.Stones);
            db.Findings.RemoveRange(company.Findings);
            db.RemoveCompany(company);
            db.SaveChanges();
            return RedirectToAction("Index");
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

                String error, warning;
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
                                    if (oxl.CellMatches("A1", worksheet, stringtable, "Style") &&
                                    (oxl.CellMatches("B1", worksheet, stringtable, "Name")) &&
                                    (oxl.CellMatches("C1", worksheet, stringtable, "JewelryType")) &&
                                    (oxl.CellMatches("D1", worksheet, stringtable, "Collection")) &&
                                    (oxl.CellMatches("E1", worksheet, stringtable, "Description")) &&
                                    (oxl.CellMatches("F1", worksheet, stringtable, "Retail")) &&
                                    (oxl.CellMatches("G1", worksheet, stringtable, "Qty")) &&
                                    (oxl.CellMatches("H1", worksheet, stringtable, "Location")))
                                    {
                                        if (worksheet.Descendants<Row>().Count() >= 2)
                                        {
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            {
                                                //process each cell in cols 1-5
                                                Style style = new Style();
                                                Collection collection = new Collection();
                                                bool bEmptyRow = true;
                                                //StyleNum
                                                style.StyleNum = "";
                                                Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    style.StyleNum = oxl.GetStringVal(cell, stringtable);
                                                }
                                                if (style.StyleNum == "")
                                                {
                                                    error = "The style number in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
                                                    ModelState.AddModelError("StyleNum-"+j, error);
                                                    ivm.Errors.Add(error);
                                                } else
                                                {
                                                    bEmptyRow = false;
                                                }
                                                // Style Name
                                                style.StyleName = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "B" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    style.StyleName = oxl.GetStringVal(cell, stringtable);
                                                }
                                                if (style.StyleName == "")
                                                {
                                                    style.StyleName = style.StyleNum;
                                                } else
                                                {
                                                    bEmptyRow = false;

                                                }

                                                // Jewelry Type - find a jewelry type with the same name or reject
                                                string JewelryTypeName = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "C" + j.ToString()).FirstOrDefault();
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
                                                    ModelState.AddModelError("JewelryType-"+j, error);
                                                    ivm.Errors.Add(error);
                                                }
                                                else
                                                {
                                                    style.JewelryTypeId = JewelryTypeId;
                                                }
                                                // Collection - find a collection with the same name in this company or reject (ie this is not a means for collection creation)
                                                string CollectionName = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "D" + j.ToString()).FirstOrDefault();
                                                CollectionName = oxl.GetStringVal(cell, stringtable);
                                                int CollectionId = GetCollectionId(CollectionName, company.Id);
                                                if (CollectionName != "")
                                                {
                                                    bEmptyRow = false;
                                                }
                                                if (CollectionId == -1)
                                                {
                                                    // add this row of this sheet to warning list
                                                    warning = "The Collection [" + CollectionName + "] in sheet [" + sheet.Name + "] row [" + j + "] does not exist; adding to default Collection";
                                                    ivm.Warnings.Add(warning);
                                                    ivm.CompanyName = db.FindCompany(ivm.CompanyId).Name;
                                                    style.CollectionId = CreateCompanyCollection(ivm, colls);
                                                }
                                                else
                                                {
                                                    style.CollectionId = CollectionId;
                                                }
                                                // Descrription
                                                style.Desc = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "E" + j.ToString()).FirstOrDefault();
                                                if (cell != null) style.Desc = oxl.GetStringVal(cell, stringtable);
                                                if (style.Desc != "")
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Retail 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "F" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    if (Decimal.TryParse(oxl.GetStringVal(cell, stringtable), out decimal rp))
                                                    {
                                                        style.RetailPrice = rp;
                                                        bEmptyRow = false;
                                                    }
                                                    else
                                                    {
                                                        error = "Invalid price [" + oxl.GetStringVal(cell, stringtable) + "] in row " + j + " of sheet [" + sheet.Name + "].";
                                                        ModelState.AddModelError("RetailPrice-"+j, error); 
                                                        ivm.Errors.Add(error);
                                                    }
                                                } else {
                                                    error = "Invalid price in row " + j + " of sheet [" + sheet.Name + "].";
                                                    ModelState.AddModelError("RetailPriceEmpty-"+j, error);
                                                    ivm.Errors.Add(error);
                                                }

                                                // Quantity 
                                                double quantity = 0;
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "G" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    quantity = oxl.GetDoubleVal(cell);
                                                    // Quality check for quantity
                                                    if (isValidQuantity(JewelryTypeName, quantity)) // integral or integral + .5
                                                    {
                                                        bEmptyRow = false;
                                                    } else {
                                                        error = "Invalid Quantity/JewelryType[" + quantity + "/" + JewelryTypeName + "] in row " + j + " of sheet [" + sheet.Name + "]. Quantity must be whole number or half number for Earrrings";
                                                        ModelState.AddModelError("Quantity-" + j, error);
                                                        ivm.Errors.Add(error);
                                                    }
                                                }
                                                else
                                                {
                                                    error = "Invalid Quantity in row " + j + " of sheet [" + sheet.Name + "].";
                                                    ModelState.AddModelError("Quantity-"+j, error);
                                                    ivm.Errors.Add(error);
                                                }

                                                // Location
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "H" + j.ToString()).FirstOrDefault();
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
                                                        Presenter companyPresenter = company.Presenters.Where(p => p.ShortName.Trim() == presenter).SingleOrDefault();
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

                                                if (bEmptyRow)
                                                {
                                                    error = "Row [" + j + "] will be ignored - All fields are blank";
                                                    ivm.Warnings.Add(error);
                                                    if (ModelState.Remove("StyleNum-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 5);
                                                    if (ModelState.Remove("JewelryType-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 4);
                                                    if (ModelState.Remove("RetailPrice-" + j) || ModelState.Remove("RetailPriceEmpty-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 3);
                                                    if (ModelState.Remove("Quantity-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 2);
                                                    if (ModelState.Remove("Location-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 1);
                                                }
                                                else
                                                {
                                                    styles.Add(style);
                                                }
                                            }
                                        }
                                        else
                                        { // row count < 2
                                            error = "The spreadsheet [" + sheet.Name + "] is formatted correctly, but does not contain any data.\n";
                                            ModelState.AddModelError("AddPostedFile-No Rows", error);
                                            ivm.Errors.Add(error);
                                        }
                                    }
                                    else
                                    { // incorrect headers
                                        error = "The sheet [" + sheet.Name + "] does not have the correct headers. Please use the New Style Template";
                                        ModelState.AddModelError("AddPostedFile-No Headers", error);
                                        ivm.Errors.Add(error);
                                    }
                                }
                                else
                                {
                                    // empty sheet
                                    error = "The sheet [" + sheet.Name + "] is empty. Please use the 'New Style' Template";
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
                                foreach (Style s in styles)
                                {
                                    Collection c;
                                    if (s.CollectionId == -1) {
                                        s.CollectionId = NewCollectionId;
                                    }
                                    c = db.Collections.Find(s.CollectionId);
                                    int count = db.Styles.Where(x => x.StyleNum == s.StyleNum && x.CollectionId == c.Id).Count();
                                    if (count == 1)
                                    {
                                        Style sty = db.Styles.Where(x => x.StyleNum == s.StyleNum && x.CollectionId == c.Id).Single();
                                        sty.Quantity += s.Quantity;
                                        foreach (Memo m in s.Memos)
                                        {
                                            Memo oldMemo = db.Memos.Where(x => x.PresenterID == m.PresenterID && x.StyleID == sty.Id).SingleOrDefault();
                                            if (oldMemo == null)
                                            {
                                                sty.Memos.Add(m);
                                            } else
                                            {
                                                oldMemo.Quantity += m.Quantity;
                                            }
                                        }
                                    }
                                    // new sytle
                                    if (count == 0)
                                    {
                                        db.Styles.Add(s);
                                    }
                                    if (count > 1)
                                    {
                                        error = "Multiple styles [" + s.StyleNum + "]. Count = [" + count + "] in collection [" + s.CollectionId + "].";
                                        ivm.Errors.Add(error);
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
                                        (oxl.CellMatches("A1", worksheet, stringtable, "Style") &&
                                        oxl.CellMatches("B1", worksheet, stringtable, "Qty") &&
                                        oxl.CellMatches("C1", worksheet, stringtable, "Retail")))
                                    {
                                        if (worksheet.Descendants<Row>().Count() >= 2)
                                        {
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            {
                                                //process each cell in cols 1-4
                                                Style style = new Style();
                                                bool bEmptyRow = true;
                                                //StyleNum
                                                style.StyleNum = "";
                                                Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null) style.StyleNum = oxl.GetStringVal(cell, stringtable);
                                                if (style.StyleNum == "")
                                                {
                                                    // error
                                                    error = "The style number in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    bEmptyRow = false;
                                                }
                                                else
                                                {
                                                    // error
                                                    error = "The Quantity in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
                                                    ModelState.AddModelError("Quantity-" + j, error);
                                                    ivm.Errors.Add(error);
                                                }

                                                // Retail Can be ignored
                                                /*
                                                string retail = "";
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "C" + j.ToString()).FirstOrDefault();
                                                if (cell != null) retail = oxl.GetStringVal(cell, stringtable);
                                                */


                                                // if whole row is blank, remove errors and flag as warning, don't add the style.
                                                if (bEmptyRow)
                                                {
                                                    // Remove last two Model Errors, add warning
                                                    error = "Row [" + j + "] will be ignored - Style Number and Quantity are blank";
                                                    ivm.Warnings.Add(error);
                                                    if (ModelState.Remove("StyleNum-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 2);
                                                    if (ModelState.Remove("Quantity-" + j)) ivm.Errors.RemoveAt(ivm.Errors.Count - 1);
                                                }
                                                else {
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
                                Style theStyle = db.Styles.Include("Collection").Include("JewelryType").Where(x => x.StyleNum == s.StyleNum).Where(y => y.Collection.CompanyId == ivm.CompanyId).SingleOrDefault();

                                if (theStyle == null)
                                {
                                    error = "The style [" + s.StyleNum + "] is not on record.";
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
                                    error = "Too few items (" + (fromMemo == null ? 0 : fromMemo.Quantity) + ") in style '" + s.StyleNum + "' at " + f.Name + " - cannot move " + moveQty + ".";
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

        public FileResult ExportInventoryReport(int? CompanyId, string currDate)
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
                    string docDate = currDate.Replace("_", ":");

                    // Date row
                    row = new Row();
                    cell = oxl.SetCellVal("A1", "Inventory Report");
                    row.Append(cell);
                    cell = oxl.SetCellVal("B1", "Date: " + docDate); 
                    row.Append(cell);
                    sd.Append(row);
                    // Header row
                    // Save Col A for image
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A2", "Style"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B2", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C2", "Description"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D2", "Jewelry Type"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 5, Max = 5, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E2", "Collection"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 6, Max = 6, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F2", "Retail"); row.Append(cell);
                    //cell = oxl.SetCellVal("G2", irm.CompanyName); row.Append(cell);
                    //cell = oxl.SetCellVal("G2", "QOH"); row.Append(cell);
                    for (int i = 0; i < irm.locations.Count(); i++)
                    {
                        ch = (char)(((int)'G') + i);
                        loc = ch + "2";
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
                        rr = 3 + i;
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
                    StyleNum = x.StyleNum,
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
                    StyleNum = x.StyleNum,
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
                (s, m) => new { m.PresenterID, m.Quantity, s.Id, s.StyleNum, sQty = s.Quantity }).Join(
                db.Presenters,
                x => x.PresenterID,
                p => p.Id,
                (x, p) => new
                {
                    StyleId = x.Id,
                    PresenterId = p.Id,
                    StyleNum = x.StyleNum,
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
                    StyleNum = x.StyleNum,
                    LocationName = x.LocationName,
                    MemoQty = x.MemoQty,
                    StyleQuantity = x.StyleQuantity
                }).
                Distinct().OrderBy(x => x.LocationName).OrderBy(x => x.StyleNum).ToList();
            irm.CompanyId = CompanyId;
            irm.CompanyName = db.FindCompany(CompanyId).Name;

            return irm;
        }

        public ActionResult ManageStoneInventory(int companyId)
        {
            StoneInventoryModel sim = new StoneInventoryModel();
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
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                            {
                                                // Read Stone, Size, Shape
                                                string stone = "", shape="", size ="", vendor = "";
                                                int delta = 0;
                                                //process each cell in cols 1-4
                                                bool bEmptyRow = true;
                                                // Stone
                                                Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).FirstOrDefault();
                                                if (cell != null) stone = oxl.GetStringVal(cell, stringtable);
                                                if (stone == "")
                                                {
                                                    // error
                                                    error = "The stone in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    error = "The shape in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    error = "The size in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    error = "The Vendor in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    bEmptyRow = false;
                                                }


                                                // if whole row is blank, remove errors and flag as warning, don't add the style.
                                                if (bEmptyRow)
                                                {
                                                    // Remove last two Model Errors, add warning
                                                    error = "Row [" + j + "] will be ignored - it contains blank cells";
                                                    sim.Warnings.Add(error);
                                                    string s = sim.Errors.Find(x => x == "Stone-" + j);
                                                    if (ModelState.Remove("Stone-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 4);
                                                    if (ModelState.Remove("Shape-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 3);
                                                    if (ModelState.Remove("Size-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 2);
                                                    if (ModelState.Remove("Vendor-" + j)) sim.Errors.RemoveAt(sim.Errors.Count - 1);
                                                }
                                                else
                                                {
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
                                    .Where(x => x.Name == s.stone && x.Shape.Name == s.shape && x.StoneSize == s.size && 
                                        x.CompanyId == sim.CompanyId).Where(x => x.Vendor == null || x.Vendor.Name == null ||
                                        (x.Vendor != null && x.Vendor.Name != "" && x.Vendor.Name == s.vendorName))
                                    .SingleOrDefault();

                                if (theStone == null)
                                {
                                    error = "The stone in row [" + s.lineNum + "] is not on record.";
                                    sim.Errors.Add(error);
                                    continue;
                                }
                                // update quantity
                                if (theStone.Qty + s.delta >= 0)
                                {
                                    theStone.Qty += s.delta;
                                } else {
                                    error = $"Insufficient inventory ({theStone.Qty}) to remove {s.delta} stones in row {s.lineNum}";
                                    sim.Errors.Add(error);
                                }
                            }
                            // Done processing, update db
                            if (sim.Errors.Count() == 0)
                            {
                                db.SaveChanges();
                                ViewBag.Message += sim.PostedFile.FileName + " inventory updated";
                            }
                        }
                    }
                }
            } catch (Exception e) {
                sim.Errors.Add($"Fatal exception:{e.InnerException}\n{e.StackTrace}");
                ModelState.AddModelError("Caught fatal exception", e);
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
            FindingInventoryModel fim = new FindingInventoryModel();
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
                                            for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
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
                                                    error = "The finding in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    error = "The Vendor in sheet [" + sheet.Name + "] row [" + j + "] is blank.";
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
                                                    bEmptyRow = false;
                                                }


                                                // if whole row is blank, remove errors and flag as warning, don't add the style.
                                                if (bEmptyRow)
                                                {
                                                    // Remove last two Model Errors, add warning
                                                    error = "Row [" + j + "] will be ignored - it contains blank cells";
                                                    fim.Warnings.Add(error);
                                                    string s = fim.Errors.Find(x => x == "Stone-" + j);
                                                    if (ModelState.Remove("Finding-" + j)) fim.Errors.RemoveAt(fim.Errors.Count - 2);
                                                    if (ModelState.Remove("Vendor-" + j)) fim.Errors.RemoveAt(fim.Errors.Count - 1);
                                                }
                                                else
                                                {
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
                                    .Where(x => x.Name == f.finding && x.CompanyId == fim.CompanyId)
                                    .ToList();
                                Finding theFinding = these.Where(x => x.Vendor == null || x.Vendor.Name == null || 
                                        (x.Vendor != null && x.Vendor.Name != "" && x.Vendor.Name == f.vendorName))
                                    .SingleOrDefault();

                                if (theFinding == null)
                                {
                                    error = "The finding in row [" + f.lineNum + "] is not on record.";
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
                                    error = $"Insufficient inventory ({theFinding.Qty}) to remove {f.delta} finding in row {f.lineNum}";
                                    fim.Errors.Add(error);
                                }
                            }
                            // Done processing, update db
                            if (fim.Errors.Count() == 0)
                            {
                                db.SaveChanges();
                                ViewBag.Message += fim.PostedFile.FileName + " inventory updated";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                fim.Errors.Add($"Fatal exception:{e.InnerException}\n{e.StackTrace}");
                ModelState.AddModelError("Caught fatal exception", e);
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
            if (CollectionId == -1 && ivm.bCC_CompCollCreated == false)
            {
                // Make new collection
                Collection c = new Collection()
                {
                    CompanyId = ivm.CompanyId,
                    Name = ivm.CompanyName,
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

