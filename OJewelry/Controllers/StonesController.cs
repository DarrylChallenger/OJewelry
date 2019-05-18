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
    public class StonesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Stones
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            StoneSorter stoneSorter = new StoneSorter();
            companyId = companyId.Value;
            var stones = db.Stones.Where(st => st.CompanyId == companyId).Include(s => s.Company).Include(s => s.Shape).Include(s => s.Vendor);
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return View(stones.AsEnumerable().OrderBy(g => g.Name).ThenBy(h=>h.Shape.Name).ThenBy(i=>i.StoneSize, stoneSorter).ToList());
        }

        // GET: Stones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stone stone = db.Stones.Find(id);
            if (stone == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;
            return View(stone);
        }
        
        // GET: Stones/Create
        public ActionResult Create(int? companyId)
        {
            if (companyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.FindCompany(companyId);
            ViewBag.CompanyId = companyId;
            ViewBag.ShapeId = new SelectList(db.Shapes.Where(s => s.CompanyId == companyId), "Id", "Name");
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == companyId), "Id", "Name", company.defaultStoneVendor);
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            Stone stone = new Stone
            {
                CompanyId = companyId,
                VendorId = company.defaultStoneVendor,
                Qty = 0
            };
            return View(stone);
        }

        // POST: Stones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,VendorId,Name,CtWt,StoneSize,ShapeId,Price,SettingCost,Qty,Note,ParentHandle,Title,Tags")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                db.Stones.Add(stone);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = stone.Id });
            }

            ViewBag.CompanyId = stone.CompanyId;
            ViewBag.ShapeId = new SelectList(db.Shapes.Where(s => s.CompanyId == stone.CompanyId), "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == stone.CompanyId), "Id", "Name", stone.VendorId);
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;
            return View(stone);
        }

        // GET: Stones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stone stone = db.Stones.Find(id);
            if (stone == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShapeId = new SelectList(db.Shapes.Where(s => s.CompanyId == stone.CompanyId), "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == stone.CompanyId), "Id", "Name", stone.VendorId);
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;
            return View(stone);
        }

        // POST: Stones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,CtWt,StoneSize,ShapeId,Price,Qty,SettingCost,Qty,Note,ParentHandle,Title,Tags")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = stone.CompanyId });
            }
            ViewBag.CompanyId = stone.CompanyId;
            ViewBag.ShapeId = new SelectList(db.Shapes.Where(s => s.CompanyId == stone.CompanyId), "Id", "Name", stone.ShapeId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == stone.CompanyId), "Id", "Name", stone.VendorId);
            ViewBag.CompanyName = db._Companies.Find(stone.CompanyId)?.Name;

            return View(stone);
        }

        // GET: Stones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stone stone = db.Stones.Find(id);
            if (stone == null)
            {
                return HttpNotFound();
            }
            return View(stone);
        }

        // POST: Stones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stone stone = db.Stones.Find(id);
            int companyId = stone.CompanyId.Value;
            db.Stones.Remove(stone);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId = companyId});
        }

        public FileResult ExportStonesReport(int companyId)
        {
            byte[] b;
            DCTSOpenXML oxl = new DCTSOpenXML();
            StoneSorter stoneSorter = new StoneSorter();
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
                    int rr;

                    Sheet sheet = new Sheet()
                    {
                        Id = workbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Stones"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Headers
                    row = new Row();
                    UInt32 cn = 1;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Name"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Title"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Stone"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Shape"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Vendor"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Carat"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Size"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Price"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Setting Cost"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Qty"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Parent Handle"); row.Append(cell); cn++;
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = cn, Max = cn, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal(oxl.GetCellName(cn, 1), "Tags"); row.Append(cell); cn++;
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<Stone> Stones = db.Stones.Include("Vendor").Include("Shape").Where(v => v.CompanyId == companyId)
                        .OrderBy(s => s.Name).ThenBy(s => s.Shape).ThenBy(s => s.StoneSize, stoneSorter).ToList();
                    // Content
                    for (int i = 0; i < Stones.Count(); i++)
                    {
                        cn = 1;
                        row = new Row();
                        rr = 2 + i;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Label); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Title); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Name); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Shape.Name); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Vendor?.Name); row.Append(cell); cn++;

                        if (Stones[i].CtWt == null)
                        {
                            cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), "");
                        } else
                        {
                            cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].CtWt.Value);
                        }
                        row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].StoneSize); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Price); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].SettingCost); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Qty); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].ParentHandle); row.Append(cell); cn++;
                        cell = oxl.SetCellVal(oxl.GetCellName(cn, rr), Stones[i].Tags); row.Append(cell); cn++;

                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Stones as of " + DateTime.Now.ToString() + ".xlsx");
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
