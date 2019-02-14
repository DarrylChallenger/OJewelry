using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OJewelry.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OJewelry.Classes;
using System.IO;

namespace OJewelry.Controllers
{
    [Authorize]
    public class JewelryTypesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: JewelryTypes
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            companyId = companyId.Value;
            var jewelryTypes = db.JewelryTypes.Where(jt => jt.CompanyId == companyId).OrderBy(jt => jt.Name);
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return View(jewelryTypes.ToList());
        }

        // GET: JewelryTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                return HttpNotFound();
            }
            return View(jewelryType);
        }

        // GET: JewelryTypes/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return View();
        }

        // POST: JewelryTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PackagingCost,FinishingCost,CompanyId")] JewelryType jewelryType)
        {
            if (ModelState.IsValid)
            {
                db.JewelryTypes.Add(jewelryType);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = jewelryType.CompanyId});
            }
            ViewBag.CompanyId = jewelryType.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(jewelryType.CompanyId)?.Name;

            return View(jewelryType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOnAddStyle(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return RedirectToAction("Create", new { companyId});
        }


        // GET: JewelryTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = jewelryType.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(jewelryType.CompanyId)?.Name;

            return View(jewelryType);
        }

        // POST: JewelryTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PackagingCost,FinishingCost,CompanyId")] JewelryType jewelryType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jewelryType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = jewelryType.CompanyId });
            }
            ViewBag.CompanyId = jewelryType.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(jewelryType.CompanyId)?.Name;
            return View(jewelryType);
        }

        // GET: JewelryTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            if (jewelryType == null)
            {
                return HttpNotFound();
            }
            return View(jewelryType);
        }

        // POST: JewelryTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JewelryType jewelryType = db.JewelryTypes.Find(id);
            // if type is in use return message
            if (db.Styles.Where(s => s.JewelryTypeId == id).Count() != 0)
            {
                ModelState.AddModelError("JewelryType", jewelryType.Name + " is in use by at least one style.");
                return View(jewelryType);
            }
            int companyId = jewelryType.CompanyId.Value;
            db.JewelryTypes.Remove(jewelryType);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId });
        }

        public FileResult ExportJewelryTypesReport(int companyId)
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
                        Name = "Jewelry Types"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Packaging Cost"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Finishing Cost"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<JewelryType> JewelryTypes = db.JewelryTypes.Where(v => v.CompanyId == companyId).OrderBy(j => j.Name).ToList();
                    // Content
                    for (int i = 0; i < JewelryTypes.Count(); i++)
                    {
                        row = new Row();
                        rr = 2 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, JewelryTypes[i].Name); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, JewelryTypes[i].PackagingCost); row.Append(cell);
                        loc = "C" + rr; cell = oxl.SetCellVal(loc, JewelryTypes[i].FinishingCost); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Jewelry Types as of " + DateTime.Now.ToString() + ".xlsx");
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
