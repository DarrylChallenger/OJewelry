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
    public class CollectionsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();
        // GET: Collections
        public ActionResult Index(int? CompanyId)
        {
            if (CompanyId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Company co = db.FindCompany(CompanyId);
            //co = db.Companies.Include("Collections").Include("Collections.Styles").Include("Collections.Styles.Memos").Where(c => c.Id == CompanyId).SingleOrDefault();
            if (co == null)
            {
                return HttpNotFound();
            }

            CollectionViewModel m = new CollectionViewModel();
            m.CompanyId = co.Id;
            m.CompanyName = co.Name;
            m.Collections = new List<CollectionModel>();
            foreach (Collection coll in co.Collections.OrderBy(c=>c.Name))
            {
                CollectionModel collM = new CollectionModel()
                {
                    Id = coll.Id,
                    CompanyId = coll.CompanyId,
                    Name = coll.Name
                };
                collM.Styles = new List<StyleModel>();
                foreach (Style sty in coll.Styles.OrderBy(s=>s.StyleName).ThenBy(s=>s.Desc))
                {
                    StyleModel styM = new StyleModel()
                    {
                        Id = sty.Id,
                        Image = sty.Image,
                        Desc = sty.Desc,
                        Name = sty.StyleName,
                        Num = sty.StyleNum,
                        Memod = sty.Memos.Sum(s => s.Quantity),
                        Qty = sty.Quantity,
                        RetialPrice = sty.RetailPrice ?? 0,
                        // Cost is the sum of the component prices
                        //Retail Price is the cost * retail ratio
                    };
                    collM.Styles.Add(styM);
                }
                m.Collections.Add(collM);
            }
            return View(m);

        }

        // GET: Collections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            Company co = db.FindCompany(collection.CompanyId);

            CollectionsDetailsModel cm = new CollectionsDetailsModel()
            {
                CompanyId = collection.CompanyId,
                CompanyName = co.Name,
                Collection = collection,
                Styles = collection.Styles.ToList(),
            };
            return View(cm);
        }

        // GET: Collections/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            CollectionModel cm = new CollectionModel()
            {
                CompanyId = id.Value,
            };
            ViewBag.CompanyName = db.FindCompany(id).Name;
            return View(cm);
        }

        // POST: Collections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CollectionModel cm)
        {
            Collection collection = new Collection(cm);
            if (ModelState.IsValid)
            {
                db.Collections.Add(collection);
                db.SaveChanges();
                return RedirectToAction("Index", new { CompanyId = collection.CompanyId });
            }

            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", collection.CompanyId);
            return View(cm);
        }

        // GET: Collections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", collection.CompanyId);
            return View(collection);
        }

        // POST: Collections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyId,Name,Notes")] Collection collection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(collection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { CompanyId = collection.CompanyId });
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "Name", collection.CompanyId);
            return View(collection);
        }

        // GET: Collections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Collection collection = db.Collections.Find(id);
            if (collection == null)
            {
                return HttpNotFound();
            }
            return View(collection);
        }

        // POST: Collections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Collection collection = db.Collections.Find(id);
            if (db.Styles.Where(s => s.CollectionId == id).Count() != 0)
            {
                ModelState.AddModelError("Collection", collection.Name + " contains at least one style.");
                return View(collection);
            }
            db.Collections.Remove(collection);
            db.SaveChanges();
            return RedirectToAction("Index", new { CompanyId = collection.CompanyId });
        }

        public FileResult ExportAssortmentReport(int companyId)
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


                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());

                    // declare locals
                    Row row;
                    Cell cell;
                    string loc;
                    int rr;
                    uint shNo = 1;
                    //for each collection
                    foreach (Collection collection in db.Collections.Where(c => c.CompanyId == companyId).OrderBy(c => c.Name))
                    {
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet(new SheetData());

                        Sheet sheet = new Sheet()
                        {
                            Id = workbookPart.GetIdOfPart(worksheetPart),
                            SheetId = shNo++,
                            Name = collection.Name // collection name"
                        };
                        sheets.Append(sheet);

                        Worksheet worksheet = new Worksheet();
                        SheetData sd = new SheetData();
                        // Build sheet
                        // Headers
                        row = new Row();
                        cell = oxl.SetCellVal("A1", "Image"); row.Append(cell);
                        cell = oxl.SetCellVal("B1", "Name"); row.Append(cell);
                        cell = oxl.SetCellVal("C1", "Desc"); row.Append(cell);
                        cell = oxl.SetCellVal("D1", "Style No."); row.Append(cell);
                        cell = oxl.SetCellVal("E1", "Inventory"); row.Append(cell);
                        sd.Append(row);
                        List<Style> Styles = db.Styles.Where(s => s.CollectionId == collection.Id).OrderBy(s => s.StyleName).ThenBy(s => s.Desc).ToList();
                        // Content
                        for (int i = 0; i < Styles.Count(); i++)
                        {
                            row = new Row();
                            rr = 2 + i;
                            loc = "A" + rr; cell = oxl.SetCellVal(loc, Styles[i].Image); row.Append(cell);
                            loc = "B" + rr; cell = oxl.SetCellVal(loc, Styles[i].StyleName); row.Append(cell);
                            loc = "C" + rr; cell = oxl.SetCellVal(loc, Styles[i].Desc); row.Append(cell);
                            loc = "D" + rr; cell = oxl.SetCellVal(loc, Styles[i].StyleNum); row.Append(cell);
                            loc = "E" + rr; cell = oxl.SetCellVal(loc, Styles[i].Quantity); row.Append(cell);
                            sd.Append(row);
                        }
                        worksheet.Append(sd);
                        // Autofit columns - ss:AutoFitWidth="1"
                        worksheetPart.Worksheet = worksheet;
                    }
                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Findings as of " + DateTime.Now.ToString() + ".xlsx");
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
