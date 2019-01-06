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
    public class ShapesController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        // GET: Shapes
        public ActionResult Index(int? companyId)
        {
            if (companyId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            companyId = companyId.Value;
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View(db.Shapes.Where(s => s.CompanyId == companyId).ToList());
        }

        // GET: Shapes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shape shape = db.Shapes.Find(id);
            if (shape == null)
            {
                return HttpNotFound();
            }
            return View(shape);
        }

        // GET: Shapes/Create
        public ActionResult Create(int companyId)
        {
            ViewBag.CompanyId = companyId;
            ViewBag.CompanyName = db._Companies.Find(companyId)?.Name;

            return View();
        }

        // POST: Shapes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,CompanyId")] Shape shape)
        {
            if (ModelState.IsValid)
            {
                db.Shapes.Add(shape);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = shape.CompanyId });
            }
            ViewBag.CompanyId = shape.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(shape.CompanyId)?.Name;

            return View(shape);
        }

        // GET: Shapes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shape shape = db.Shapes.Find(id);
            if (shape == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = shape.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(shape.CompanyId)?.Name;
            return View(shape);
        }

        // POST: Shapes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CompanyId")] Shape shape)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shape).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { shape.CompanyId });
            }
            ViewBag.CompanyId = shape.CompanyId;
            ViewBag.CompanyName = db._Companies.Find(shape.CompanyId)?.Name;
            return View(shape);
        }

        // GET: Shapes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shape shape = db.Shapes.Find(id);
            if (shape == null)
            {
                return HttpNotFound();
            }
            return View(shape);
        }

        // POST: Shapes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Shape shape = db.Shapes.Find(id);
            if (db.Stones.Where(c => c.ShapeId == id).Count() !=0)
            {
                ModelState.AddModelError("Shapes", shape.Name + " is in use by at least one stone.");
                return View(shape);
            }
            int companyId = shape.CompanyId.Value;
            db.Shapes.Remove(shape);
            db.SaveChanges();
            return RedirectToAction("Index", new { companyId });
        }

        public FileResult ExportStoneShapesReport(int companyId)
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
                        Name = "Stone Shapes"
                    };
                    sheets.Append(sheet);

                    Worksheet worksheet = new Worksheet();
                    SheetData sd = new SheetData();
                    // Build sheet
                    // Headers
                    row = new Row();
                    cell = oxl.SetCellVal("A1", "Name"); row.Append(cell);

                    sd.Append(row);
                    List<Shape> Shapes = db.Shapes.Where(v => v.CompanyId == companyId).ToList();
                    // Content
                    for (int i = 0; i < Shapes.Count(); i++)
                    {
                        row = new Row();
                        rr = 2 + i;
                        loc = "A" + rr; cell = oxl.SetCellVal(loc, Shapes[i].Name); row.Append(cell);

                        sd.Append(row);
                    }
                    worksheet.Append(sd);
                    // Autofit columns - ss:AutoFitWidth="1"
                    worksheetPart.Worksheet = worksheet;
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Stone Shapes as of " + DateTime.Now.ToString() + ".xlsx");
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
