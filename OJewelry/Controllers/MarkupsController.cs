using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
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
                if (m.ratio == 0)
                {
                    ModelState.AddModelError($"markups[{i}].ratio", "The Markup cannot be 0. ");
                }
                if (m.multiplier == 0)
                {
                    ModelState.AddModelError($"markups[{i}].multiplier", "The Multiplier cannot be 0. ");
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
