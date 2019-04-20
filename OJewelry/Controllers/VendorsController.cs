using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OJewelry.Classes;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    [Authorize]
    public class VendorsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Vendors
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            companyId = companyId.Value;
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View(db.Vendors.Include("Type").Where(v => v.CompanyId == companyId).OrderBy(v => v.Name).ToList());
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            ViewBag.TypeId = SetVendorTypesDropDown();

            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Email,TypeId,Notes,CompanyId")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Vendors.Add(vendor);
                db.SaveChanges();
            }
            ViewBag.TypeId = SetVendorTypesDropDown();
            ViewBag.CompanyId = vendor.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(vendor.CompanyId)?.Name;

            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = vendor.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(vendor.CompanyId)?.Name;
            ViewBag.TypeId = SetVendorTypesDropDown();
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Email,TypeId,Notes,CompanyId")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = vendor.CompanyId });
            }
            ViewBag.CompanyId = vendor.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(vendor.CompanyId)?.Name;
            ViewBag.TypeId = SetVendorTypesDropDown();
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Find(id);
            
            if (db.Castings.Where(c => c.VendorId == id).Count() != 0 ||
                db.Stones.Where(s => s.VendorId == id).Count() != 0 ||
                db.Findings.Where(s => s.VendorId == id).Count() != 0)
            {
                ModelState.AddModelError("Vendor", vendor.Name + " is in use by at least one casting, stone, or finding.");
                return View(vendor);
            }
            int companyId = vendor.CompanyId.Value;
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId });
        }

        protected SelectList SetVendorTypesDropDown()
        {
            SelectList s = new SelectList(db.VendorTypes, "Id", "Name");//, "-- Choose a Type --");

            return s;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult ExportVendorReport(int companyId)
        {
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
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                    // declare locals
                    Row row;
                    Cell cell;
                    string loc;
                    int rr;

                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Vendors"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Phone"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Email"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Type"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == companyId).OrderBy(vv => vv.Name).ToList();
                    // Content
                    int offset = 2;
                    for (int i = 0; i < vendors.Count(); i++)
                    {

                        if (vendors[i].Name == "") // don't print default vendor
                        {
                            offset--;
                            continue;
                        }
                        row = new Row();
                        rr = offset + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, vendors[i].Name); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, vendors[i].Phone); row.Append(cell);
                        loc = "C" + rr; cell = oxl.SetCellVal(loc, vendors[i].Email); row.Append(cell);
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, vendors[i].Notes); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Vendors as of " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

    }
}
