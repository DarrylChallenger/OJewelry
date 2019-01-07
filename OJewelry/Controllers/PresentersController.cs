using System;
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
    [Authorize]
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
            ViewBag.CompanyId = co.Id;
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
                SavePresenters(pvm.Location.Id, pvm.contacts);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = pvm.Location.CompanyId });
            }

            pvm.Location.Company = db.FindCompany(pvm.Location.CompanyId);
            ViewBag.CompanyName = pvm.Location.Name;
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
            List<Contact> contacts = db.Contacts.Where(x => x.Location.Id == pvm.Location.Id).ToList();
            foreach (Contact c in contacts)
            {
                PresenterViewContactModel pvcm = new PresenterViewContactModel(c);
                pvm.contacts.Add(pvcm);
            }
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
            pvm.Location.Company = db.FindCompany(pvm.Location.CompanyId);
            if (ModelState.IsValid)
            {
                db.Entry(pvm.Location).State = EntityState.Modified;
                SavePresenters(pvm.Location.Id, pvm.contacts);
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
            // only allow delete if there is more than one presenter for this company
            if (db.Presenters.Where(p => p.CompanyId == presenter.CompanyId).Count() < 2)
            {
                ViewBag.Message = "This is the only location for this company so it cannot be removed";
                return View(presenter);
            }
            /*  
            foreach (Memo m in db.Memos.Where(mm => mm.PresenterID == id))
            {
            }
            */
            int numMemos = db.Memos.Where(mm => mm.PresenterID == id).Count();
            if (numMemos != 0)
            {
                ViewBag.Message = "There are items in this location; it cannot be deleted.";
                return View(presenter);
            }
            else
            {
                int coid = presenter.CompanyId;
                List<Contact> contacts = new List<Contact>();
                foreach (Contact c in db.Contacts.Where(c => c.PresenterId == id))
                {
                    contacts.Add(c);
                }
                db.Contacts.RemoveRange(contacts);
                db.Presenters.Remove(presenter);
                db.SaveChanges();
                return RedirectToAction("Index", new { companyId = coid });
            }
        }

        void SavePresenters(int presenterId, List<PresenterViewContactModel> contacts)
        {
            Contact con;
            foreach (PresenterViewContactModel p in contacts)
            {
                if (p.Name != null)
                {
                    p.PresenterId = presenterId;
                    switch (p.State)
                    {
                        case PVCMState.Added:
                            db.Contacts.Add(p.GetContact());
                            break;
                        case PVCMState.Deleted:
                            con = p.GetContact();
                            db.Entry(con).State = EntityState.Deleted;
                            db.Contacts.Remove(con);
                            break;
                        case PVCMState.Dirty:
                            con = p.GetContact();
                            db.Entry(con).State = EntityState.Modified;
                            break;
                        case PVCMState.Unadded:
                        case PVCMState.Clean:
                        default:
                            break;
                    }
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
