﻿using System;
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
    public class MetalCodesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: MetalCodes
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            companyId = companyId.Value;
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View(db.MetalCodes.Where(mc => mc.CompanyId == companyId).OrderBy(mc=>mc.Code).ToList());
        }

        // GET: MetalCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetalCode metalCode = db.MetalCodes.Find(id);
            if (metalCode == null)
            {
                return HttpNotFound();
            }
            return View(metalCode);
        }

        // GET: MetalCodes/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View();
        }

        // POST: MetalCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Desc,Market,Multiplier,CompanyId")] MetalCode metalCode)
        {
            if (ModelState.IsValid)
            {
                db.MetalCodes.Add(metalCode);
                db.SaveChanges();
                return RedirectToAction("Index", new { metalCode.CompanyId });
            }
            ViewBag.CompanyId = metalCode.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(metalCode.CompanyId)?.Name;

            return View(metalCode);
        }

        // GET: MetalCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MetalCode metalCode = db.MetalCodes.Find(id);
            if (metalCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = metalCode.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(metalCode.CompanyId)?.Name;
            return View(metalCode);
        }

        // POST: MetalCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Desc,Market,Multiplier,CompanyId")] MetalCode metalCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metalCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { metalCode.CompanyId });
            }
            ViewBag.CompanyId = metalCode.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(metalCode.CompanyId)?.Name;
            return View(metalCode);
        }

        // GET: MetalCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeleteMetalCodeModel dmcm = new DeleteMetalCodeModel()
            {
                item = db.MetalCodes.Find(id)
            };
            if (dmcm.item == null)
            {
                return HttpNotFound();
            }
            if (db.Castings.Where(s => s.MetalCodeID == id).Count() != 0)
            {
                // List the styles
                List<Style> styles = new List<Style>();
                styles = db.StyleCastings.Where(sc => sc.Casting.MetalCodeID == id).Select(sc => sc.Style).ToList();
                dmcm.styles = styles.Distinct(new StyleEqualityComparer()).ToList();
                dmcm.bError = true;
            }
            return View(dmcm);
        }

        // POST: MetalCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MetalCode metalCode = db.MetalCodes.Find(id);

            int companyId = metalCode.CompanyId.Value;
            db.MetalCodes.Remove(metalCode);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId });
        }

        public FileResult ExportMetalsReport(int companyId, string sCurrDate)
        {
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
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                    // declare locals
                    Row row;
                    Cell cell;
                    string loc;
                    int rr;
                    // Build sheet
                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Metals"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Title
                    row = new Row();
                    cell = oxl.SetCellVal("A1", $"Export - Metal Codes  {currDate}");
                    row.Append(cell);
                    sd.Append(row);
                    row = new Row();
                    cell = oxl.SetCellVal("A2", "");
                    row.Append(cell);
                    sd.Append(row);

                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A3", "Code"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B3", "Desc"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C3", "Market"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D3", "Multiplier"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<MetalCode> Metals = db.MetalCodes.Where(m => m.CompanyId == companyId).OrderBy(m => m.Code).ToList();
                    // Content
                    for (int i = 0; i < Metals.Count(); i++)
                    {
                        row = new Row();
                        rr = 4 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, Metals[i].Code); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, Metals[i].Desc); row.Append(cell);
                        loc = "C" + rr; cell = oxl.SetCellVal(loc, Metals[i].Market); row.Append(cell);
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, Metals[i].Multiplier); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Metals as of " + $"{currDate}" + ".xlsx");
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
