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
using Newtonsoft.Json;
using OJewelry.Classes;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    public class MarkupsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Markups
        public ActionResult Index(int? CompanyId)
        {
            if (CompanyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.FindCompany(CompanyId);
            if (company == null)
            {
                return HttpNotFound();
            }
            MarkupModel mm = new MarkupModel()
            {
                CompanyId = company.Id,
                CompanyName = company.Name
            };
            if (company.markup != null)
            {
                mm.markups = JsonConvert.DeserializeObject<List<Markup>>(company.markup);
            }
            return View(mm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MarkupModel mm)
        {
            int i = 0;
            foreach(Markup m in mm.markups)
            {
                if (m.ratio < 0)
                {
                    ModelState.AddModelError($"markups[{i}].ratio", "The Markup (%) cannot be tess than 0. ");
                }
                if (m.margin < 0)
                {
                    ModelState.AddModelError($"markups[{i}].margin", "The Margin (%) cannot be less than 0. ");
                }
                if (m.margin > 100)
                {
                    ModelState.AddModelError($"markups[{i}].margin", "The Margin (%) cannot be more than 100. ");
                }
                if (m.multiplier == 0)
                {
                    ModelState.AddModelError($"markups[{i}].multiplier", "The Multiplier cannot be 0. ");
                }
                if (m.ratio !=0 && m.margin !=0)
                {
                    ModelState.AddModelError($"markups[{i}].ratio", "You can only use one of Markup and Margin. ");
                }
                i++;
            }
            if (ModelState.IsValid)
            {
                // Save in Company
                Company company = db.FindCompany(mm.CompanyId);
                List<Markup> markupsToSave = new List<Markup>();
                foreach (Markup m in mm.markups)
                {
                    switch (m.State)
                    {
                        case MMState.Added:
                        case MMState.Clean:
                        case MMState.Dirty:
                            m.State = MMState.Dirty;
                            markupsToSave.Add(m);
                            break;
                        default:
                            break;
                    }
                }
                company.markup = JsonConvert.SerializeObject(markupsToSave);
                db.SaveChanges();
                return RedirectToAction("Index", "Companies");
            } else
            {
                return View(mm);
            }
        }

        public FileResult ExportMarkups(int companyId, string sCurrDate)
        {
            byte[] b;

            DateTime curr;
            sCurrDate = sCurrDate.Replace("'", "");
            if (!DateTime.TryParse(sCurrDate, out curr))
            {
                curr = DateTime.Now.ToLocalTime();
            }
            string currDate = $"{curr.ToShortDateString()} {curr.ToShortTimeString()}";

            Company company = db.FindCompany(companyId);
            MarkupModel mm = new MarkupModel();
            if (company.markup != null)
            {
                mm.markups = JsonConvert.DeserializeObject<List<Markup>>(company.markup);
            }


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
                        Name = "Markups"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Title
                    row = new Row();
                    cell = oxl.SetCellVal("A1", $"Export - Markups  {currDate}");
                    row.Append(cell);
                    sd.Append(row);
                    row = new Row();
                    cell = oxl.SetCellVal("A2", "");
                    row.Append(cell);
                    sd.Append(row);
                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A3", "Title"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B3", "Multiplier"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C3", "Markup"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D3", "Margin"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E3", "Addend"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    // Content
                    for (int i = 0; i < mm.markups.Count(); i++)
                    {
                        Markup m = mm.markups[i];
                        row = new Row();
                        rr = 4 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, m.title); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, m.multiplier); row.Append(cell);
                        loc = "C" + rr; cell = oxl.SetCellVal(loc, m.ratio); row.Append(cell);
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, m.margin); row.Append(cell);
                        loc = "E" + rr; cell = oxl.SetCellVal(loc, m.Addend); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Markups as of {currDate}.xlsx");
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
