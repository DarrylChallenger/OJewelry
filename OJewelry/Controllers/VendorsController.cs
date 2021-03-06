﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
            List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == companyId).OrderBy(v => v.Name).ToList();
            vendors.ForEach(v => v.Phone = SetFormattedPhone(v.Phone));
            return View(vendors);
        }

        // GET: Vendors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vendor vendor = db.Vendors.Where( v => v.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: Vendors/Create
        [HttpGet]
        public ActionResult Create(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company company = db.FindCompany(companyId);
            if (company == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vendor vendor = new Vendor();
            VendorViewModel vvm = new VendorViewModel(vendor);
            vvm.CompanyName = company?.Name;
            vvm.CompanyId = companyId;
            return View(vvm);
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorViewModel vvm)
        {
            if (ModelState.IsValid)
            {
                Vendor vendor = new Vendor(vvm);
                db.Vendors.Add(vendor);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = vendor.CompanyId });
            }

            return View(vvm);
        }

        // GET: Vendors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Vendor vendor = db.Vendors.Where(v => v.Id == id).FirstOrDefault();
            if (vendor == null)
            {
                return HttpNotFound();
            }
            VendorViewModel vvm = new VendorViewModel(vendor);
            Company company = db.FindCompany(vendor.CompanyId);
            vvm.Phone = SetFormattedPhone(vendor.Phone);
            if (company == null)
            {
                return RedirectToAction("Index", "Home");
            }
            vvm.CompanyName = company?.Name;

            return View(vvm);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorViewModel vvm)
        {
            if (ModelState.IsValid)
            {
                Vendor vendor = new Vendor(vvm);
                db.Entry(vendor).State = EntityState.Modified;
                vendor.Phone = GetNormalizedPhone(vvm.Phone);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = vendor.CompanyId });
            }
            return View(vvm);
        }

        // GET: Vendors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DeleteVendorModel dvm = new DeleteVendorModel()
            {
                item = db.Vendors.Where(v => v.Id == id).FirstOrDefault()
            };
            if (dvm.item == null)
            {
                return HttpNotFound();
            }
            if (db.Castings.Where(c => c.VendorId == id).Count() != 0 ||
                    db.LaborTable.Where(li => li.VendorId == id).Count() != 0 ||
                    db.Stones.Where(s => s.VendorId == id).Count() != 0 ||
                    db.Findings.Where(s => s.VendorId == id).Count() != 0)
            {
                // List the labors, castings, stones, and findings
                dvm.castings.AddRange(db.Castings.Where(c => c.VendorId == id).ToList().Distinct(new CastingEqualityComparer()));
                dvm.stones.AddRange(db.Stones.Where(s => s.VendorId == id).ToList().Distinct(new StoneEqualityComparer()));
                dvm.findings.AddRange(db.Findings.Where(f => f.VendorId == id).ToList().Distinct(new FindingEqualityComparer()));
                dvm.labors.AddRange(db.Labors.Where(l => l.VendorId == id).ToList().Distinct(new LaborEqualityComparer()));
                dvm.laborItems.AddRange(db.LaborTable.Where(li => li.VendorId == id).ToList().Distinct(new LaborItemEqualityComparer()));
                //IEnumerable<Style> sty = styles.Distinct(new StyleEqualityComparer());
                /*foreach (Style s in sty)
                {
                    dvm.styles.Add(s);
                }*/
                dvm.bError = true;
            }
            return View(dvm);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendor vendor = db.Vendors.Where(v => v.Id == id).FirstOrDefault();

            int companyId = vendor.CompanyId.Value;
            db.Vendors.Remove(vendor);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId });
        }

        protected SelectList SetVendorTypesDropDown(Vendor v)
        {
            SelectList s = null;
            IEnumerable<VendorTypeEnumObj> VendorTypes = v.GetEnumOjbs();

            if (v.Type != null)
            {
                s = new SelectList(VendorTypes, "Id", "Name", v.Type);//, "-- Choose a Type --");
            } else
            {
                s = new SelectList(VendorTypes, "Id", "Name", 0);//, "-- Choose a Type --");
            }
            return s;
        }

        private string GetNormalizedPhone(string phone)
        {
            if (phone == "" || phone == null) { return phone; }
            string[] newPhone = Regex.Split(phone, "[.()-]");
            string finPhone = "";
            for (int i = 0; i < newPhone.Length; i++)
            {
                finPhone += newPhone[i];
            }
            return finPhone;
        }

        private string SetFormattedPhone(string phone)
        {
            if (phone == "" || phone == null) return "";
            string newPhone = Regex.Replace(phone, @"^([0-9]{3})([0-9]{3})([0-9]{4})$", @"$1-$2-$3");
            return newPhone;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult ExportVendorReport(int companyId, string sCurrDate)
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

                    workbookPart.AddNewPart<WorkbookStylesPart>();
                    workbookPart.WorkbookStylesPart.Stylesheet = oxl.CreateStyleSheet();
                    workbookPart.WorkbookStylesPart.Stylesheet.Save();

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
                    // Title
                    row = new Row();
                    cell = oxl.SetCellVal("A1", $"Export - Vendors  {currDate}");
                    row.Append(cell);
                    sd.Append(row);
                    row = new Row();
                    cell = oxl.SetCellVal("A2", "");
                    row.Append(cell);
                    sd.Append(row);
                    // Headers
                    row = new Row();
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A3", "Name"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B3", "Phone"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C3", "Email"); row.Append(cell);
                    oxl.columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D3", "Sells"); row.Append(cell);
                    worksheet.Append(oxl.columns);
                    sd.Append(row);
                    List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == companyId).OrderBy(vv => vv.Name).ToList();
                    // Content
                    int offset = 4;
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
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, vendors[i].Type.Type.ToString()); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Vendors as of " + $"{currDate}" + ".xlsx");
                }
            }
        }
    }
}
