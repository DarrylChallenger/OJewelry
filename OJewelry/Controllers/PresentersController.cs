﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using OJewelry.Models;
using OJewelry.Classes;

namespace OJewelry.Controllers
{
    public class PresentersController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Presenters
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company co = db.FindCompany(companyId);
            if (co == null)
            {
                return HttpNotFound();
            }
            var presenters = db.Presenters.Where(x => x.CompanyId == companyId).OrderBy(p=>p.Name).Include(p => p.Company);
            ViewBag.CompanyName = co.Name;
            ViewBag.CompanyId = co.Id;
            return View(presenters.ToList());
        }

        // GET: Presenters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presenter presenter = db.Presenters.Find(id);
            presenter.Company = db.FindCompany(presenter.CompanyId);
            if (presenter == null)
            {
                return HttpNotFound();
            }
            return View(presenter);
        }

        // GET: Presenters/Create
        public ActionResult Create(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Company co = db.FindCompany(companyId);
            if (co == null)
            {
                return HttpNotFound();
            }
            PresenterViewModel pvm = new PresenterViewModel();
            ViewBag.CompanyName = co.Name;
            pvm.Location.CompanyId = co.Id;
            return View(pvm);
        }

        // POST: Presenters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PresenterViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                db.Presenters.Add(pvm.Location);
                foreach (Contact c in pvm.contacts)
                {
                    if (c.Id == 0)
                    {
                        if (c.Name != null)
                        {
                            c.PresenterId = pvm.Location.Id;
                            db.Contacts.Add(c);
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
                return RedirectToAction("Index", new { companyId = pvm.Location.CompanyId });
            }

            pvm.Location.Company = db.FindCompany(pvm.Location.CompanyId);
            return View(pvm);
        }

        // GET: Presenters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PresenterViewModel pvm = new PresenterViewModel();
            pvm.Location = db.Presenters.Find(id);
            if (pvm.Location == null)
            {
                return HttpNotFound();
            }
            pvm.contacts = db.Contacts.Where(c => c.PresenterId == id).ToList();
            pvm.contacts.Add(new Contact());
            pvm.Location.Company = db.FindCompany(pvm.Location.CompanyId);

            return View(pvm);
        }

        // POST: Presenters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PresenterViewModel pvm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pvm.Location).State = EntityState.Modified;
                foreach (Contact c in pvm.contacts)
                {
                    if (c.Id == 0)
                    {
                        if (c.Name != null)
                        {
                            c.PresenterId = pvm.Location.Id;
                            db.Contacts.Add(c);
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
                return RedirectToAction("Index", new { companyId = pvm.Location.CompanyId });
            }
            pvm.Location.Company = db.FindCompany(pvm.Location.CompanyId);
            return View(pvm);
        }

        // GET: Presenters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Presenter presenter = db.Presenters.Find(id);
            presenter.Company = db.FindCompany(presenter.CompanyId);
            if (presenter == null)
            {
                return HttpNotFound();
            }
            return View(presenter);
        }

        // POST: Presenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Presenter presenter = db.Presenters.Find(id);
            int coid = presenter.CompanyId.Value;
            db.Presenters.Remove(presenter);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId = coid });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public FileResult ExportLocationReport(int CompanyId)
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
                        Name = "Locations" 
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Headers
                    row = new Row();
                    cell = oxl.SetCellVal("A1", "Name"); row.Append(cell);
                    cell = oxl.SetCellVal("B1", "Short Name"); row.Append(cell);
                    cell = oxl.SetCellVal("C1", "Phone"); row.Append(cell);
                    cell = oxl.SetCellVal("D1", "Email"); row.Append(cell);
                    sd.Append(row);
                    List<Presenter> locations = db.Presenters.Where(x => x.CompanyId == CompanyId).ToList();
                    // Content
                    for (int i = 0; i < locations.Count(); i++)
                    {
                        row = new Row();
                        rr = 2 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, locations[i].Name); row.Append(cell);
                        loc = "B" + rr; cell = oxl.SetCellVal(loc, locations[i].ShortName); row.Append(cell);
                        loc = "C" + rr; cell = oxl.SetCellVal(loc, locations[i].Phone); row.Append(cell);
                        loc = "D" + rr; cell = oxl.SetCellVal(loc, locations[i].Email); row.Append(cell);
                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    Company company = db.FindCompany(CompanyId);
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        company.Name + " Locations as of " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}
