using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            List<Vendor> vendors = db.Vendors.Where(v => v.CompanyId == CompanyId).ToList();
            foreach (LaborItem li in ltm.Labors)
            {
                li.selectList = new SelectList(vendors, "Id", "Name", li.VendorId);
            }

            /*
            ltm.Labors.Add(new LaborItem
            {
                Name = "test",
                pph = 10.01M,
                ppp = 20.02M,
                State = LMState.Added,
                CompanyId = CompanyId.Value,
                selectList = new SelectList(db.Vendors.Where(x => x.CompanyId == CompanyId), "Id", "Name", 0)
            });
            */
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
                vendors = db.Vendors.Where(v => v.CompanyId == ltm.CompanyId).ToList();
                foreach (LaborItem li in ltm.Labors)
                {
                    li.selectList = new SelectList(vendors, "Id", "Name", li.VendorId);

                }
                return View(ltm);
            }
            vendors = db.Vendors.Where(v => v.CompanyId == ltm.CompanyId).ToList();

            foreach (LaborItem li in ltm.Labors)
            {
                    li.selectList = new SelectList(vendors, "Id", "Name", li.VendorId);

            }
            return View(ltm);
        }
    }
}
