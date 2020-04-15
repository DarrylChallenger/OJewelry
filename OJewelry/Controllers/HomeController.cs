using OJewelry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact BOSS";

            return View();
        }

        [Authorize]
        public ActionResult ClientList()
        {
            ViewBag.Message = "Client List";
            OJewelryDB dc = new OJewelryDB();
            var aClient = dc.Clients.Join(dc.Companies, cli => cli.CompanyID, com => com.Id,
                (cli, com) => new
                {
                    cli.Id,
                    ClientName = cli.Name,
                    ClientPhone = cli.Phone,
                    ClientEmail = cli.Email,
                    CompanyName = com.Name,
                    CompanyId = com.Id
                }).ToList().Select(n => new ClientViewModel()
                { 
                    Id = n.Id,
                    Name = n.ClientName,
                    Phone = n.ClientPhone,
                    Email = n.ClientEmail,
                    CompanyName = n.CompanyName,
                    CompanyId = n.CompanyId
                 }
                ).ToList();
            return View("ClientList", aClient);
        }

        public ActionResult CollectionListByCompany(int id)
        {
            ViewBag.Message = "Collection List for company";

            OJewelryDB dc = new OJewelryDB();
            CollectionViewModel m = new CollectionViewModel();
            Company co = dc.FindCompany(id);
            m.CompanyId = co.Id;
            m.CompanyName = co.Name;
            m.Collections = new List<CollectionModel>();
            foreach (Collection coll in  co.Collections)
            {
                CollectionModel collM = new CollectionModel()
                {
                    Id = coll.Id,
                    CompanyId = coll.CompanyId,
                    Name = coll.Name
                };
                collM.Styles = new List<StyleModel>();
                foreach (Style sty in coll.Styles)
                {
                    StyleModel styM = new StyleModel()
                    {
                        Id = sty.Id,
                        Image = sty.Image,
                        Name = sty.StyleName,
                        //Num = sty.StyleNum,
                        Qty = sty.Quantity,
                        Memod = sty.Memos.Sum(s => s.Quantity)
                        // Cost is the sum of the component prices
                        //Retail Price is the cost * retail ratio
                    };
                    collM.Styles.Add(styM);
                }
                m.Collections.Add(collM);
            }
            return View(m);
        }

        [Authorize]
        public ActionResult MemoStyle(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return ClientList();
            }

            // Move a Style in or out of Memo
            OJewelryDB dc = new OJewelryDB();
            Style style = dc.Styles.Find(Id);
            MemoViewModel m = new MemoViewModel();
            m.style = new StyleModel()
            {
                Id = style.Id,
                Image = style.Image,
                Name = style.StyleName,
                //Num = style.StyleNum,
                Qty = style.Quantity,
                Memod = style.Memos.Sum(s => s.Quantity)
            };
            m.SendReturnMemoRadio = 1;
            m.NewExistingPresenterRadio = 1;

            m.Memos = new List<MemoModel>();
            m.numPresentersWithStyle = 0;
            foreach(Memo i in dc.Memos)
            {
                MemoModel mm = new MemoModel()
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    date = i.Date,
                    PresenterName = i.Presenter.Name,
                    PresenterPhone = i.Presenter.Phone,
                    PresenterEmail = i.Presenter.Email,
                    Notes = i.Notes
                };
                m.Memos.Add(mm);
                m.numPresentersWithStyle++;
            }
            m.Presenters = new List<SelectListItem>();
            m.CompanyId = style.Collection.CompanyId;
            foreach (Presenter i in dc.Presenters.Where(w=>w.CompanyId == m.CompanyId))
            {
                SelectListItem sli = new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                };
                m.Presenters.Add(sli);
            }
            m.PresenterName = "initname";
            return View(m);
        }

        /*
        [HttpPost]
        public ActionResult MemoStyle(FormCollection form)
        {
            OJewelryDBEntities dc = new OJewelryDBEntities();
            if (form["SendReturnMemoRadio"] == "1")
            {
                if (form["NewExistingPresenterRadio"] == "2")
                {
                    //Memo a new presenter
                    if (String.IsNullOrEmpty(form["PresenterName"]))
                    {
                        ModelState.AddModelError("Presenter Name", "Name is required for new Presenters.");
                    }
                    if (String.IsNullOrEmpty(form["PresenterEmail"]) && String.IsNullOrEmpty(form["PresenterPhone"]))
                    {
                        ModelState.AddModelError("Presenter Contact Info", "Phone or Email is required for new Presenters.");
                    }
                }
                else
                {
                    // Memo an existing presenter - nothing to validate in this case as they just selected a Presenter from the list
                }
                int i;
                if (Int32.TryParse(form["SendQty"], out i) == true)
                {
                    if (i == 0)
                    {
                        ModelState.AddModelError("Send Quantity", "The send quantity should be the number of items to memeo.");
                    }
                    int styleID;
                    if (Int32.TryParse(form["style.Id"], out styleID))
                    {
                        int dbStyleQty = dc.Styles.Find(styleID).Quantity;
                        if (i > dbStyleQty)
                        {
                            ModelState.AddModelError("Send Quantity", "You cannot memo more items than you have in inventory.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Serious Error", "Data was corrupted. (HSMSFormStyleId");
                    }
                }
            }
            else
            {
                //Return Items from Presenter
                //numPresentersWithStyle

            }
            if (ModelState.IsValid)
            {
                // Save changes, go to clientlist
                return ClientList();
            }
            // rebuild model
            //GetPresenters(dc, m, CompanyId);
            return View();
        }
        */


        [Authorize]
        [HttpPost]
        public ActionResult MemoStyle(MemoViewModel m)
        {
            ModelState.Clear();
            OJewelryDB dc = new OJewelryDB();
            // populate style data
            Style sdb = dc.Styles.Find(m.style.Id);
            m.style.Name = sdb.StyleName;
            //m.style.Num = sdb.StyleNum;
            m.style.Qty = sdb.Quantity;

            if (m.SendReturnMemoRadio == 1)
            {
                if (m.NewExistingPresenterRadio == 2)
                {
                    //Memo a new presenter
                    if (String.IsNullOrEmpty(m.PresenterName))
                    {
                        ModelState.AddModelError("Presenter Name", "Name is required for new Presenters.");
                    }
                    if (String.IsNullOrEmpty(m.PresenterEmail) && String.IsNullOrEmpty(m.PresenterPhone))
                    {
                        ModelState.AddModelError("Presenter Contact Info", "Phone or Email is required for new Presenters.");
                    }
                    Presenter p = new Presenter()
                    {
                        CompanyId = m.CompanyId,
                        Name = m.PresenterName,
                        Email = m.PresenterEmail,
                        Phone = m.PresenterPhone,
                    };
                    dc.Presenters.Add(p);
                    m.PresenterId = p.Id;
                }
                else
                {
                    // Memo an existing presenter - nothing to validate in this case as they just selected a Presenter from the list
                }
                // create new memo, reduce inventory
                sdb.Quantity -= m.SendQty;
                String note = "Sending " + m.SendQty.ToString() + " items to " + m.PresenterName + " on " + DateTime.Now.ToString();
                Memo mo = new Memo()
                {
                    Quantity = m.SendQty,
                    Date = DateTime.Now,
                    Notes = note, 
                    PresenterID = m.PresenterId,
                    StyleID = m.style.Id,
                };
                dc.Memos.Add(mo);
                if (m.SendQty > m.style.Qty)
                {
                    ModelState.AddModelError("Send Quantity", "You cannot memo more items than you have in inventory.");
                }
                if (m.SendQty < 1)
                {
                    ModelState.AddModelError("Send Quantity", "You can only memo a positive number of items.");
                }
            }
            else
            {
                //Return Items from Presenter
                // iterate thru the memos to take items back. Increase the inventory as appropriate. If all items are returned, delete the memo
                foreach(MemoModel memo in m.Memos)
                {
                    if (memo.ReturnQty < 0)
                    {
                        ModelState.AddModelError("Return Style", "You can only return a positive number to inventory.");
                    }
                    if (memo.ReturnQty > 0)
                    {
                        if (memo.ReturnQty > memo.Quantity)
                        {
                            ModelState.AddModelError("Return Style", "You can't return more items than were memo'd out.");
                        }
                        // update db
                        Memo mdb = dc.Memos.Find(memo.Id);
                        if (mdb.Quantity == memo.ReturnQty)
                        {
                            // remove the row
                            dc.Memos.Remove(mdb);
                        } else
                        {
                            // decrease the amount
                            mdb.Quantity -= memo.ReturnQty; 
                        }
                        sdb.Quantity += memo.ReturnQty;
                    }
                    // ReturnQty is 0, no action
                }

            }
            if (ModelState.IsValid)
            {
                // Save changes, go to clientlist
                dc.SaveChanges();
                return ClientList();
            }
            // Re-present the page to allow for corrections
            GetPresenters(dc, m, m.CompanyId);

            int numPresentersWithStyle = m.numPresentersWithStyle;
            m.numPresentersWithStyle = 0;
            Dictionary<int, int> d = new Dictionary<int, int>();
            for (int ii = 0; ii < numPresentersWithStyle; ii++)
            {
                d.Add(m.Memos[ii].Id, ii);
            }

            foreach (Memo memo in dc.Memos)
            {
                if (d.ContainsKey(memo.Id))
                {
                    memo.Presenter = dc.Presenters.Find(memo.PresenterID);
                    m.Memos[d[memo.Id]].Quantity = memo.Quantity;
                    m.Memos[d[memo.Id]].date = memo.Date;
                    m.Memos[d[memo.Id]].PresenterName = memo.Presenter.Name;
                    m.Memos[d[memo.Id]].PresenterPhone = memo.Presenter.Phone;
                    m.Memos[d[memo.Id]].PresenterEmail = memo.Presenter.Email;
                    m.Memos[d[memo.Id]].Notes = memo.Notes;
                    m.style.Memod += dc.Memos.Find(memo.Id).Quantity;
                }
                else
                {
                    MemoModel mm = new MemoModel()
                    {
                        Id = memo.Id,
                        Quantity = memo.Quantity,
                        date = memo.Date,
                        PresenterName = memo.Presenter.Name,
                        PresenterPhone = memo.Presenter.Phone,
                        PresenterEmail = memo.Presenter.Email,
                        Notes = memo.Notes
                    };
                    m.style.Memod += dc.Memos.Find(memo.Id).Quantity;

                    m.Memos.Add(mm);
                }
                m.numPresentersWithStyle++;
            }
            return View(m);
        }

        [Authorize]
        void GetPresenters(OJewelryDB dc, MemoViewModel m, int CompanyId)
        {
            m.Presenters = new List<SelectListItem>();
            foreach (Presenter i in dc.Presenters.Where(w => w.CompanyId == CompanyId))
            {
                SelectListItem sli = new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString(),
                };
                m.Presenters.Add(sli);
            }
        }
    }
}