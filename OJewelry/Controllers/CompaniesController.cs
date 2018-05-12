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
using DocumentFormat.OpenXml;

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
                CompanyUser cu = new CompanyUser()
                {
                    CompanyId = cvm.company.Id,
                    UserId = user.Id
                };
                cvm.company.CompanyUsers.Add(cu);
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
            List<Client> clients = db.Clients.Where(x => x.CompanyID == cvm.company.Id).ToList();
            foreach(Client c in clients)
            {
                CompanyViewClientModel cvcm =  new CompanyViewClientModel(c);
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

                foreach (CompanyViewClientModel c in cvm.clients)
                {
                    Client client;
                    switch (c.State)
                    {
                        case CVCMState.Added:
                            client = new Client(c, cvm.company.Id);
                            db.Clients.Add(client);
                            break;
                        case CVCMState.Deleted:
                            client = new Client(c, cvm.company.Id);
                            db.Entry(client).State = EntityState.Deleted;
                            //db.Clients.Remove(client);
                            break;
                        case CVCMState.Dirty:
                            client = new Client(c, cvm.company.Id);
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
                ModelState.AddModelError("Company", company.Name + " has at least one collection that is not empty.");
                return View(company);
            }
            // Remove collections, locations, clients, components
//            List<Collection> collections = db.Collections.Where(col => col.CompanyId == id).ToList();
  //          db.Collections.RemoveRange(collections);
            db.Collections.RemoveRange(company.Collections);
            db.Presenters.RemoveRange(company.Presenters);
            db.Clients.RemoveRange(company.Clients);
            db.Components.RemoveRange(company.Components);
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
                            foreach (Sheet sheet in wb.Sheets)
                            {
                                WorksheetPart wsp = (WorksheetPart)wbPart.GetPartById(sheet.Id);
                                Worksheet worksheet = wsp.Worksheet;
                                int rc = worksheet.Descendants<Row>().Count();
                                if (rc != 0)
                                {
                                    if (CellMatches("A1", worksheet, stringtable, "Style") &&
                                    (CellMatches("B1", worksheet, stringtable, "Name")) &&
                                    (CellMatches("C1", worksheet, stringtable, "JewelryType")) &&
                                    (CellMatches("D1", worksheet, stringtable, "Collection")) &&
                                    (CellMatches("E1", worksheet, stringtable, "Description")) &&
                                    (CellMatches("F1", worksheet, stringtable, "Retail")) &&
                                    (CellMatches("G1", worksheet, stringtable, "Qty")) &&
                                    (CellMatches("H1", worksheet, stringtable, "Location")))
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
                                                    style.StyleNum = GetStringVal(cell, stringtable);
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
                                                    style.StyleName = GetStringVal(cell, stringtable);
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
                                                    JewelryTypeName = GetStringVal(cell, stringtable);
                                                }
                                                int JewelryTypeId = GetJewelryTypeId(JewelryTypeName);
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
                                                CollectionName = GetStringVal(cell, stringtable);
                                                int CollectionId = GetCollectionId(CollectionName);
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
                                                if (cell != null) style.Desc = GetStringVal(cell, stringtable);
                                                if (style.Desc != "")
                                                {
                                                    bEmptyRow = false;
                                                }

                                                // Retail 
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "F" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    if (Decimal.TryParse(GetStringVal(cell, stringtable), out decimal rp))
                                                    {
                                                        style.RetailPrice = rp;
                                                        bEmptyRow = false;
                                                    }
                                                    else
                                                    {
                                                        error = "Invalid price [" + GetStringVal(cell, stringtable) + "] in row " + j + " of sheet [" + sheet.Name + "].";
                                                        ModelState.AddModelError("RetailPrice-"+j, error); 
                                                        ivm.Errors.Add(error);
                                                    }
                                                } else {
                                                    error = "Invalid price in row " + j + " of sheet [" + sheet.Name + "].";
                                                    ModelState.AddModelError("RetailPriceEmpty-"+j, error);
                                                    ivm.Errors.Add(error);
                                                }

                                                // Quantity 
                                                int quantity = 0;
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "G" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    quantity = GetIntVal(cell);
                                                    bEmptyRow = false;
                                                }
                                                else {
                                                    error = "Invalid Quantity in row " + j + " of sheet [" + sheet.Name + "].";
                                                    ModelState.AddModelError("Quantity-"+j, error);
                                                    ivm.Errors.Add(error);
                                                }

                                                // Location
                                                cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "H" + j.ToString()).FirstOrDefault();
                                                if (cell != null)
                                                {
                                                    string presenter = GetStringVal(cell, stringtable);
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
                                                        Company company = db.FindCompany(ivm.CompanyId);
                                                        Presenter companyPresenter = company.Presenters.Where(p => p.ShortName == presenter).SingleOrDefault();
                                                        if (companyPresenter != null)
                                                        {
                                                            // yes, add the memo
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
                                                    // add to QOH
                                                    // flag as error
                                                    error = "Location in row " + j + " of sheet [" + sheet.Name + "] is blank.";
                                                    ModelState.AddModelError("Location-" + j, error);
                                                    ivm.Errors.Add(error);
                                                    //style.Quantity = quantity;
                                                    //bEmptyRow = false;
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
                                            sty.Memos.Add(m);
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
                                        (CellMatches("A1", worksheet, stringtable, "Style") &&
                                        CellMatches("B1", worksheet, stringtable, "Qty") &&
                                        CellMatches("C1", worksheet, stringtable, "Retail")))
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
                                                if (cell != null) style.StyleNum = GetStringVal(cell, stringtable);
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
                                                    style.Quantity = GetIntVal(cell);
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
                                                if (cell != null) retail = GetStringVal(cell, stringtable);
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
                                Style theStyle = db.Styles.Include("Collection").Where(x => x.StyleNum == s.StyleNum).Where(y => y.Collection.CompanyId == ivm.CompanyId).SingleOrDefault();

                                if (theStyle == null)
                                {
                                    error = "The style [" + s.StyleNum + "] is not on record.";
                                    ivm.Errors.Add(error);
                                    continue;
                                }
                                // update desc and retail if blank in DB
                                if (theStyle.Desc == null || theStyle.Desc == "") theStyle.Desc = s.Desc;
                                // should we always update retail?
                                //if (theStyle.Retail == null) theStyle.Retail = s.Retail;  
                                //// if moving from home
                                int moveQty = s.Quantity;

                                // from -= qty, to += qty
                                Memo fromMemo, toMemo;
                                // if moving from home, create a memo. If moving to home, decrease memo qty/remove the memo
                                if (ivm.FromLocationId == 0) // get the quant from syles
                                {
                                    if (moveQty <= theStyle.Quantity)
                                    {
                                        theStyle.Quantity -= moveQty;

                                    }
                                    else
                                    {
                                        // if qty in from < qty, error
                                        error = "Too few items (" + theStyle.Quantity + ") in style [" + s.StyleNum + "] - cannot move " + moveQty + ".";
                                        ivm.Errors.Add(error);
                                        continue;
                                    }
                                }
                                else
                                {
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
                                }
                                // if moving to home
                                if (ivm.ToLocationId == 0)
                                {
                                    theStyle.Quantity += moveQty;
                                }
                                else if (ivm.ToLocationId == -1)
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
                    cell = SetCellVal("A1", "Inventory Report");
                    row.Append(cell);
                    cell = SetCellVal("B1", "Date: " + docDate); 
                    row.Append(cell);
                    sd.Append(row);
                    // Header row
                    // Save Col A for image
                    row = new Row();
                    cell = SetCellVal("A2", "Style"); row.Append(cell);
                    cell = SetCellVal("B2", "Name"); row.Append(cell);
                    cell = SetCellVal("C2", "Description"); row.Append(cell);
                    cell = SetCellVal("D2", "Jewelry Type"); row.Append(cell);
                    cell = SetCellVal("E2", "Collection"); row.Append(cell);
                    cell = SetCellVal("F2", "Retail"); row.Append(cell);
                    //cell = SetCellVal("G2", irm.CompanyName); row.Append(cell);
                    //cell = SetCellVal("G2", "QOH"); row.Append(cell);
                    for (int i = 0; i < irm.locations.Count(); i++)
                    {
                        ch = (char)(((int)'G') + i);
                        loc = ch + "2";
                        cell = SetCellVal(loc, irm.locations[i].ShortName);
                        row.Append(cell);
                    }
                    /* Ignore Sold
                    ch = (char)(((int)'G') + irm.locations.Count());
                    loc = ch + "2";
                    cell = SetCellVal(loc, "SOLD");
                    */
                    row.Append(cell);
                    sd.Append(row);

                    // Loop thru styles
                    for (int i = 0; i < irm.styles.Count; i++)
                    {
                        row = new Row();
                        rr = 3 + i;
                        loc = "A" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].StyleNum); row.Append(cell);
                        loc = "B" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].StyleName); row.Append(cell);
                        loc = "C" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].StyleDesc); row.Append(cell);
                        loc = "D" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].JewelryTypeName); row.Append(cell);
                        loc = "E" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].StyleCollectionName); row.Append(cell);
                        loc = "F" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].StylePrice); row.Append(cell);
                        //loc = "G" + rr.ToString(); cell = SetCellVal(loc, irm.styles[i].StyleQuantity); row.Append(cell);

                        for (int j = 0; j < irm.locations.Count; j++)
                        {
                            ch = (char)(((int)'G') + j);
                            loc = ch.ToString() + rr.ToString();
                            irmLS irmls = irm.locationQuantsbystyle.
                                Where(x => x.StyleId == irm.styles[i].StyleId && x.PresenterId == irm.locations[j].PresenterId).SingleOrDefault();
                            if (irmls != null)
                            {
                                cell = SetCellVal(loc, irmls.MemoQty);
                            }
                            else
                            {
                                cell = SetCellVal(loc, 0);
                            }
                            row.Append(cell);
                           
                        }
                        /* Ignore SOLD
                        ch = (char)(((int)'G') + irm.locations.Count());
                        loc = ch.ToString() + rr.ToString();
                        cell = SetCellVal(loc, irm.styles[i].StyleQtySold); row.Append(cell);
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        bool CellMatches(string cellref, Worksheet w, SharedStringTablePart strings, string value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        if (strings.SharedStringTable.ElementAt(int.Parse(cell.CellValue.InnerText)).InnerText == value)
                        {
                            return true;
                        }
                    }
                }
            } catch
            {
                return false;
            }
            return false;
        }

        bool CellMatches(string cellref, Worksheet w, int value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType == null)
                {
                    if (int.Parse(cell.CellValue.InnerText) == value)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        bool CellMatches(string cellref, Worksheet w, DateTime value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.Date)
                    {
                        if ((DateTime.Parse(cell.CellValue.InnerText)) == value)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        bool CellMatches(string cellref, Worksheet w, bool value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.Boolean)
                    {
                        if (Boolean.Parse(cell.CellValue.InnerText) == value)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        string GetStringVal(Cell cell, SharedStringTablePart strings)
        {
            String str = "";
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        str = strings.SharedStringTable.ElementAt(int.Parse(cell.CellValue.InnerText)).InnerText;
                    }
                    if (cell.DataType == CellValues.String)
                    {
                        str = cell.CellValue.Text;
                    }
                }
                else
                {
                    str = cell.CellValue.Text;
                }
            }
            catch
            {
                str = "";
            }
            return str;
        }

        int GetIntVal(Cell cell)
        {
            int i = 0;
            try
            {
                if (cell.DataType == null)
                {
                    i = int.Parse(cell.CellValue.InnerText);
                }
            }
            catch
            {
                return 0;
            }

            return i;
        }

        Cell SetCellVal(string loc, int val)
        {

            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            return cell;
        }

        Cell SetCellVal(string loc, decimal val)
        {

            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            return cell;
        }

        Cell SetCellVal(string loc, string val)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.String, CellValue = new CellValue(val) };
            return cell;
        }

        int GetJewelryTypeId(string JewelryTypeName)
        {
            JewelryType jt = db.JewelryTypes.Where(j => j.Name == JewelryTypeName).FirstOrDefault();
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

        int GetCollectionId(string CollectionName)
        {
            Collection co = db.Collections.Where(c => c.Name == CollectionName).FirstOrDefault();
            if (co == null)
            {
                return -1;
            }
            return co.Id;
        }

        int CreateCompanyCollection(InventoryViewModel ivm, List<Collection> colls)
        {
            int CollectionId = GetCollectionId(ivm.CompanyName);
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

    }
}

