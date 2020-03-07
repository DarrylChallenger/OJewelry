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
    public class FindingsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Findings
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            companyId = companyId.Value;
            var findings = db.Findings.Where(st => st.CompanyId == companyId).Include(f => f.Company).Include(f => f.Vendor);
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return View(findings.OrderBy(f => f.Name).ToList());
        }

        // GET: Findings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finding finding = db.Findings.Find(id);
            if (finding == null)
            {
                return HttpNotFound();
            }
            return View(finding);
        }

        // GET: Findings/Create
        public ActionResult Create(int? companyId)
        {
            if (companyId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.FindCompany(companyId);
            if (company == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == companyId && ((v.Type.Type & vendorTypeEnum.Finding) == vendorTypeEnum.Finding)), "Id", "Name");
            Finding finding = new Finding
            {
                CompanyId = companyId
            };
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;
            return View(finding);
        }

        // POST: Findings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,Price,Weight,Qty,Note")] Finding finding)
        {
            Finding existingFinding = db.Findings.Where(f => f.CompanyId == finding.CompanyId && f.Name == finding.Name).FirstOrDefault();
            if (existingFinding != null)
            {
                ModelState.AddModelError("Name", $"A finding named '{finding.Name}' already exists. ");
            }
            if (ModelState.IsValid)
            {
                db.Findings.Add(finding);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = finding.Id });
            }

            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == finding.CompanyId && (v.Type.Type & vendorTypeEnum.Finding) == vendorTypeEnum.Finding), "Id", "Name", finding.VendorId);
            ViewBag.CompanyName = db._Companies.Find(finding.CompanyId)?.Name;
            return View(finding);
        }

        // GET: Findings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finding finding = db.Findings.Find(id);
            if (finding == null)
            {
                return HttpNotFound();
            }
            //ViewBag.MetalCodeId = new SelectList(db.MetalCodes, "Id", "Code", finding.MetalCodeId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == finding.CompanyId && (v.Type.Type & vendorTypeEnum.Finding) == vendorTypeEnum.Finding), "Id", "Name", finding.VendorId);
            ViewBag.CompanyName = db._Companies.Find(finding.CompanyId)?.Name;
            return View(finding);
        }

        // POST: Findings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,VendorId,Name,Desc,Price,Weight,Qty,Note")] Finding finding)
        {
            Finding existingFinding = db.Findings.Where(f => f.Id != finding.Id && f.CompanyId == finding.CompanyId && f.Name == finding.Name).FirstOrDefault();
            if (existingFinding != null)
            {
                ModelState.AddModelError("Name", $"A finding named '{finding.Name}' already exists. ");
            }
            if (ModelState.IsValid)
            {
                db.Entry(finding).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = finding.CompanyId });
            }
            //ViewBag.MetalCodeId = new SelectList(db.MetalCodes, "Id", "Code", finding.MetalCodeId);
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == finding.CompanyId && ((v.Type.Type & vendorTypeEnum.Finding) == vendorTypeEnum.Finding)), "Id", "Name", finding.VendorId);
            ViewBag.CompanyName = db._Companies.Find(finding.CompanyId)?.Name;
            return View(finding);
        }

        // GET: Findings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Finding finding = db.Findings.Find(id);
            if (finding == null)
            {
                return HttpNotFound();
            }
            return View(finding);
        }

        // POST: Findings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Finding finding = db.Findings.Find(id);
            int companyId = finding.CompanyId.Value;
            db.Findings.Remove(finding);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId = companyId });
        }

        [HttpPost, ActionName("ShowCopy")]
        [ValidateAntiForgeryToken]
        public ActionResult ShowCopy(Finding finding)
        {
            Finding newFinding = new Finding(finding);
            ModelState.Clear();
            newFinding.Name = "Copy of " + finding.Name;
            newFinding.Qty = 0;
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == finding.CompanyId && ((v.Type.Type & vendorTypeEnum.Finding) == vendorTypeEnum.Finding)), "Id", "Name", finding.VendorId);
            ViewBag.CompanyName = db._Companies.Find(finding.CompanyId)?.Name;
            return View("Copy", newFinding);
        }

        [HttpPost, ActionName("Copy")]
        [ValidateAntiForgeryToken]
        public ActionResult Copy(Finding finding)
        {
            Finding newFinding = new Finding(finding);
            Finding existingFinding = db.Findings.Where(f => f.CompanyId == finding.CompanyId && f.Name == finding.Name).FirstOrDefault();
            if (existingFinding != null)
            {
                ModelState.AddModelError("Name", $"A finding named '{finding.Name}' already exists. ");
            }
            if (ModelState.IsValid)
            {
                db.Findings.Add(finding);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = finding.CompanyId });
            }
            ViewBag.VendorId = new SelectList(db.Vendors.Where(v => v.CompanyId == finding.CompanyId && ((v.Type.Type & vendorTypeEnum.Finding) == vendorTypeEnum.Finding)), "Id", "Name", finding.VendorId);
            ViewBag.CompanyName = db._Companies.Find(finding.CompanyId)?.Name;
            return View(finding);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ConfirmRemoveInventory(int companyId)
        {
            Company co = db.FindCompany(companyId);
            if (co == null)
            {
                RedirectToAction("Index", "Home");
            }
            List<Finding> findings = db.Findings.Where(f => f.CompanyId == companyId).ToList(); 
            ViewBag.companyId = companyId;
            ViewBag.CompanyName = co.Name;
            return View(findings);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveInventory(int companyId)
        {
            Company co = db.FindCompany(companyId);
            if (co == null)
            {
                RedirectToAction("Index", "Home");
            }

            List<Finding> findings = db.Findings.Where(f => f.CompanyId == companyId).ToList();
            foreach (Finding f in findings)
            {
                f.Qty = 0;
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Findings", new { companyId });
        }

        public FileResult ExportFindingsReport(int companyId)
        {
            byte[] b;
            DateTime now = DateTime.Now;

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
                        Name = "Findings"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Title
                    row = new Row();
                    cell = oxl.SetCellVal("A1", $"Export - Findings {now.ToLocalTime().ToShortDateString()} {now.ToLocalTime().ToShortTimeString()}");
                    row.Append(cell);
                    sd.Append(row);
                    row = new Row();
                    cell = oxl.SetCellVal("A2", "");
                    row.Append(cell);
                    sd.Append(row);
                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A3", "Id"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B3", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C3", "Weight (dwt)"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D3", "Price"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 5, Max = 5, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E3", "Vendor"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 6, Max = 6, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F3", "Inventory"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<Finding> Findings = db.Findings.Where(v => v.CompanyId == companyId).OrderBy(f => f.Name).Include("Vendor").ToList();
                    // Content
                    for (int i = 0; i < Findings.Count(); i++)
                    {
                        row = new Row();
                        rr = 4 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, Findings[i].Id); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, Findings[i].Name); row.Append(cell);
                        loc = "C" + rr; 
                        if (Findings[i].Weight == null)
                        {
                            cell = oxl.SetCellVal(loc, "");
                        }
                        else
                        {
                            cell = oxl.SetCellVal(loc, Findings[i].Weight.Value);
                        }
                        row.Append(cell);
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, Findings[i].Price); row.Append(cell);
                        loc = "E" + rr; cell = oxl.SetCellVal(loc, Findings[i].Vendor.Name); row.Append(cell);
                        loc = "F" + rr; cell = oxl.SetCellVal(loc, Findings[i].Qty); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"Findings as of {now.ToLocalTime().ToShortDateString()} {now.ToLocalTime().ToShortTimeString()}.xlsx");
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
