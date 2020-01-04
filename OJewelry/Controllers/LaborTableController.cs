using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OJewelry.Classes;
using OJewelry.Models;

namespace OJewelry.Controllers
{
    public class LaborTableController : Controller
    {
        //private ApplicationDbContext sec = new ApplicationDbContext();
        private OJewelryDB db = new OJewelryDB();

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
            // Check for Labor Vendor
            LaborTableModel ltm = new LaborTableModel()
            {
                Labors = db.LaborTable.Where(l => l.CompanyId == CompanyId).ToList(),
                CompanyId = CompanyId.Value,
                CompanyName = company.Name,
            };
            List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == CompanyId && (v.Type.Type & vendorTypeEnum.Labor) == vendorTypeEnum.Labor).ToList();
            foreach (LaborItem li in ltm.Labors)
            {
                li.selectList = new SelectList(vendors, "Id", "Name", li.VendorId);
            }
            ltm.bHasVendors = vendors.Count != 0;
            return View(ltm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LaborTableModel ltm)
        {
            List<Vendor> vendors;
            if (ModelState.IsValid)
            {
                int i = -1;
                //db.Entry(cvm.company).State = EntityState.Modified;

                foreach (LaborItem li in ltm.Labors)
                {
                    i++;
                    switch (li.State)
                    {
                        case LMState.Added:
                            li.CompanyId = ltm.CompanyId;
                            db.LaborTable.Add(li);
                            li.State = LMState.Dirty;
                            break;
                        case LMState.Deleted:
                            // Make sure its not used. If it is, reset to "Dirty" and add an error message
                            if (db.StyleLaborItems.Where(sli => sli.LaborTableId == li.Id).Count() > 0)
                            {
                                li.State = LMState.Dirty;
                                ModelState.SetModelValue($"Labors[{i}].State", new ValueProviderResult("Dirty", "", System.Globalization.CultureInfo.InvariantCulture));
                                ModelState.AddModelError($"Labors[{i}].Name", $"You cannot delete {li.Name}: it is used by styles.");
                                break;
                            }
                            db.Entry(li).State = EntityState.Deleted;
                            db.LaborTable.Remove(li);
                            break;
                        case LMState.Dirty:
                            db.Entry(li).State = EntityState.Modified;
                            break;
                        case LMState.Unadded:
                        case LMState.Clean:
                        case LMState.Fixed:
                            break;
                    }
                }

            }
            vendors = db.Vendors.Where(v => v.CompanyId == ltm.CompanyId).ToList();
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                int CompanyId = ltm.CompanyId;
                string CompanyName = ltm.CompanyName;
                ModelState.Clear();
                ltm = new LaborTableModel()
                {
                    Labors = db.LaborTable.Where(l => l.CompanyId == CompanyId).ToList(),
                    CompanyId = CompanyId,
                    CompanyName = CompanyName,
                };
                foreach (LaborItem li in ltm.Labors)
                {
                    li.selectList = new SelectList(vendors, "Id", "Name", li.VendorId);

                }
                ltm.bHasVendors = vendors.Count != 0;
                return View(ltm);
            }
            foreach (LaborItem li in ltm.Labors)
            {
                li.selectList = new SelectList(vendors, "Id", "Name", li.VendorId);

            }
            ltm.bHasVendors = vendors.Count != 0;
            return View(ltm);
        }

        public FileResult ExportLaborTable(int companyId)
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
                        Name = "Labor Table"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "$/Hour"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "$/Piece"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Vendor"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<LaborItem> LaborTableItems = db.LaborTable.Where(lt => lt.CompanyId == companyId).OrderBy(lt => lt.Name).ToList();
                    // Content
                    for (int i = 0; i < LaborTableItems.Count(); i++)
                    {
                        row = new Row();
                        rr = 2 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, LaborTableItems[i].Name); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, LaborTableItems[i].pph.GetValueOrDefault()); row.Append(cell);
                        loc = "C" + rr; cell = oxl.SetCellVal(loc, LaborTableItems[i].ppp.GetValueOrDefault()); row.Append(cell);
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, LaborTableItems[i].Vendor.Name); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();
                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Labor Table as of " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}
