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

namespace OJewelry.Controllers
{
    public class CompaniesController : Controller
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private OJewelryDB db = new OJewelryDB();

        // GET: Companies
        public ActionResult Index()
        {
            return View(db.Companies.ToList());
        }

        // GET: Companies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Company company)
        {
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.Companies.Find(id);
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
            Company company = db.Companies.Find(id);
            db.Companies.Remove(company);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            ivm.CompanyName = db.Companies.Find(CompanyId).Name;
            ivm.FromLocations = new SelectList(db.Presenters, "Id", "Name");
            ivm.ToLocations = new SelectList(db.Presenters, "Id", "Name");
            return View("Inventory", ivm);
        }

        [HttpPost]
        public ActionResult AddInventory(InventoryViewModel ivm)
        {
            try
            {
                String error;
                if (ModelState.IsValid)
                {
                    // open file

                    Package spreadsheetPackage = Package.Open(ivm.AddPostedFile.FileName, FileMode.Open, FileAccess.Read);

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
                            WorksheetPart wsp = (WorksheetPart) wbPart.GetPartById(sheet.Id);
                            Worksheet worksheet = wsp.Worksheet;
                            if (CellMatches("A1", worksheet, stringtable, "Style") &&
                                (CellMatches("B1", worksheet, stringtable, "Name")) &&
                                (CellMatches("C1", worksheet, stringtable, "JewelryType")) &&
                                (CellMatches("D1", worksheet, stringtable, "Collection")) &&
                                (CellMatches("E1", worksheet, stringtable, "Description")) &&
                                (CellMatches("F1", worksheet, stringtable, "Retail")) &&
                                (CellMatches("G1", worksheet, stringtable, "Qty")))
                            { 
                                if (worksheet.Descendants<Row>().Count() >= 2)
                                {
                                    for (int i = 1,  j=2; i <  worksheet.Descendants<Row>().Count(); i++, j=i+1) /* Add checks for empty values */
                                    {
                                        //process each cell in cols 1-5
                                        Style style = new Style();
                                        Collection collection = new Collection();
                                        //StyleNum
                                        Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).First();
                                        style.StyleNum = GetStringVal(cell, stringtable);
                                        
                                        // Style Name
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "B" + j.ToString()).First();
                                        style.StyleName = GetStringVal(cell, stringtable);
                                        
                                        // Jewelry Type - find a jewelry type with the same name or reject
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "C" + j.ToString()).First();
                                        string JewelryTypeName = GetStringVal(cell, stringtable);
                                        int JewelryTypeId = GetJewelryTypeId(JewelryTypeName);
                                        if (JewelryTypeId == -1)
                                        {
                                            error = "The Jewelry Type [" + JewelryTypeName + "] in sheet [" + sheet.Name + "] row [" + j + "] does not exist.";
                                            ivm.Errors.Add(error);
                                            continue; // add this row of this sheet to error list
                                        } else
                                        {
                                            style.JewelryTypeId = JewelryTypeId;
                                        }
                                        // Collection - find a collection with the same name in this company or reject (ie this is not a means for collection creation)
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "D" + j.ToString()).First();
                                        string CollectionName = GetStringVal(cell, stringtable);
                                        int CollectionId = GetCollectionId(CollectionName);
                                        if (CollectionId == -1)
                                        {
                                            error = "The Collection [" + CollectionName + "] in sheet [" + sheet.Name + "] row [" + j + "] does not exist.";
                                            ivm.Errors.Add(error);
                                            continue; // add this row of this sheet to error list
                                        }
                                        else {
                                            style.CollectionId = CollectionId;
                                            // Add coll here 
                                            // colls.Add(collection);
                                        }
                                        // Descrription
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "E" + j.ToString()).First();
                                        style.Desc = GetStringVal(cell, stringtable);
                                        
                                        // Retail - Oh oh, not stored, only computed! Add new column just for retail cost :(
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "F" + j.ToString()).First();
                                        string retail = GetStringVal(cell, stringtable);
                                        
                                        // Quantity 
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "G" + j.ToString()).First();
                                        style.Quantity = GetIntVal(cell);
                                        styles.Add(style);
                                        
                                    }
                                } else { // row count < 2
                                    error = "The spreadsheet [" + sheet.Name + "] is formatted correctly, but does not contain any data.\n";
                                    ivm.Errors.Add(error);
                                }
                            } else { // incorrect headers
                                error = "The sheet [" + sheet.Name + "] does not have the correct headers. Please use the New Style Template";
                                ivm.Errors.Add(error);
                            }
                        }
                        // process collections and styles
                        // new collection
                        foreach (Collection c in colls) // should be empty for now as I'm not allowing new collections
                        {
                            if (db.Collections.Where(x => x.CompanyId == ivm.CompanyId && x.Name == c.Name).Count() == 0)
                            {
                                db.Collections.Add(c);
                            }
                        }
                        // existing style - if style already exists, just update the quant
                        foreach (Style s in styles)
                        {
                            Collection c = db.Collections.Find(s.CollectionId);
                            int count = db.Styles.Where(x => x.StyleNum == s.StyleNum && x.CollectionId == c.Id).Count();
                            if (count == 1)
                            {
                                Style sty = db.Styles.Where(x => x.StyleNum == s.StyleNum && x.CollectionId == c.Id).Single();
                                sty.Quantity += s.Quantity;
                            }
                            // new sytle
                            if (count == 0)
                            {
                                db.Styles.Add(s); 
                            }
                            if (count > 1)
                            {
                                error = "Something went wrong trying to update a style quantity [" + s.StyleNum + "]. Count = [" + count + "].";
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
            } catch (Exception e) {
                ViewBag.Message += ("Exception[" +e.ToString()+ "]");
            } finally {
                ivm.CompanyName = db.Companies.Find(ivm.CompanyId).Name;
                ivm.FromLocations = new SelectList(db.Presenters, "Id", "Name");
                ivm.ToLocations = new SelectList(db.Presenters, "Id", "Name");
            }
            return View("Inventory", ivm);
        }

        [HttpPost]
        public ActionResult MoveInventory(InventoryViewModel ivm, HttpPostedFileBase postedFile)
        {
            try
            {
                String error;
                if (ModelState.IsValid)
                {
                    // open file

                    Package spreadsheetPackage = Package.Open(ivm.AddPostedFile.FileName, FileMode.Open, FileAccess.Read);

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
                            if (CellMatches("A1", worksheet, stringtable, "Style") &&
                                CellMatches("B1", worksheet, stringtable, "Description") &&
                                CellMatches("C1", worksheet, stringtable, "Retail") &&
                                CellMatches("D1", worksheet, stringtable, "Qty"))
                            {
                                if (worksheet.Descendants<Row>().Count() >= 2)
                                {
                                    for (int i = 1, j = 2; i < worksheet.Descendants<Row>().Count(); i++, j = i + 1) /* Add checks for empty values */
                                    {
                                        //process each cell in cols 1-4
                                        Style style = new Style();

                                        //StyleNum
                                        Cell cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "A" + j.ToString()).First();
                                        style.StyleNum = GetStringVal(cell, stringtable);
                                        
                                        // Descrription
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "E" + j.ToString()).First();
                                        style.Desc = GetStringVal(cell, stringtable);

                                        // Retail - Oh oh, not stored, only computed! Add new column just for retail cost :(
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "F" + j.ToString()).First();
                                        string retail = GetStringVal(cell, stringtable);

                                        // Quantity 
                                        cell = worksheet.Descendants<Cell>().Where(c => c.CellReference == "G" + j.ToString()).First();
                                        style.Quantity = GetIntVal(cell);
                                        styles.Add(style);
                                    }
                                } else { // row count < 2
                                    error = "The spreadsheet [" + sheet.Name + "] is formatted correctly, but does not contain any data.\n";
                                    ivm.Errors.Add(error);
                                }
                            } else { // incorrect headers
                                error = "The sheet [" + sheet.Name + "] does not have the correct headers. Please use the New Style Template";
                                ivm.Errors.Add(error);
                            }
                        }
                        // process moves
                        foreach (Style s in styles)
                        {
                            Style theStyle = db.Styles.Include("Collection").Where(x => x.StyleNum == s.StyleNum && s.Collection.CompanyId == ivm.CompanyId).SingleOrDefault();
                            Presenter from, to;
                            from = db.Presenters.Find(ivm.FromLocationId);
                            to = db.Presenters.Find(ivm.ToLocationId);
                            //// if moving from home
                            //// if moving to home
                            //// if moving from and to memo'd
                            //// if selling

                            /* 
                             * note: current memos are memo transactions. 
                             * Change so that Presenters have a style/qty relationship in a new table. 
                             */


                            // find the from memos
                            int fromqty = db.Memos.Where(x => x.Id == ivm.FromLocationId).Sum(y => y.Quantity);
                            // if qty in from < qty, error
                            if (fromqty < s.Quantity)
                            {
                                error = "Too few items in style [" + s.StyleNum + "].";
                                ivm.Errors.Add(error);
                                continue;
                            }
                            // if from == to, error
                            if (ivm.FromLocationId == ivm.ToLocationId)
                            {
                                error = "You cannot from/to the same location";
                                ivm.Errors.Add(error);
                                continue;
                            }
                            // OK  ...
                            // from -= qty, to += qty
                            // update desc and retail if blank in DB
                            if (theStyle.Desc == null) theStyle.Desc = s.Desc;
                            //if (theStyle.Retail == null) theStyle.Retail = s.Retail;  // should we always update retail?

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
            catch (Exception e)
            {
                ViewBag.Message += ("Exception[" + e.ToString() + "]");
            }
            finally
            {
                ivm.CompanyName = db.Companies.Find(ivm.CompanyId).Name;
                ivm.FromLocations = new SelectList(db.Presenters, "Id", "Name");
                ivm.ToLocations = new SelectList(db.Presenters, "Id", "Name");
            }
            return View("Inventory", ivm);
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
                /*
                string s = "The Collection Type [" + CollectionName + "] does not exist.";
                ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(s);
                throw ex;
                */
                return -1;
            }
            return co.Id;
        }

    }
}

