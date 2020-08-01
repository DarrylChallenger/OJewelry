using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OJewelry.Classes;
using OJewelry.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Controllers
{
    public class HomeController : Controller
    {
        class CastingReport {
            /*
            public int styleId { get; set; }
            public string styleName { get; set; }
            public int castingId { get; set; }
            public string name { get; set; }
            public decimal? metal { get; set; }
            public decimal weight { get; set; }
            public int weightUnit { get; set; }
            public decimal? price { get; set; }
            public decimal? labor { get; set; }
            public decimal qty { get; set; }
            public string vendorName { get; set; }
            */
            public Style style { get; set; }
            public Casting casting { get; set; }
        }

        class StoneReport
        {
            public Stone stone { get; set; }
            public Style style { get; set; }
            public int? qty { get; set; }
        }
        class FindingReport
        {
            public Finding finding { get; set; }
            public Style style { get; set; }
            public decimal? qty { get; set; }
        }
        class LaborReport
        {
            public Labor labor { get; set; }
            public Style style { get; set; }
        }
        class LaborTableReport
        {
            public LaborItem laborItem { get; set; }
            public Style style { get; set; }
            public decimal qty { get; set; }
        }

        private OJewelryDB db = new OJewelryDB();
        public delegate Worksheet BuildSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId);

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
            foreach (Collection coll in co.Collections)
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
            foreach (Memo i in dc.Memos)
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
            foreach (Presenter i in dc.Presenters.Where(w => w.CompanyId == m.CompanyId))
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
                foreach (MemoModel memo in m.Memos)
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
                        }
                        else
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

        [Authorize(Roles ="Admin")]
        public ActionResult CompanyReport()
        {
            CompanyReport cr = new CompanyReport();

            Company defComp = new Company()
            {
                Name = "Select a Company",
                Id = 0
            };
            List<Company> companies = db.Companies.OrderBy(c=>c.Name).ToList();
            companies.Insert(0, defComp);
            cr.CompanyList = new SelectList(companies, "Id", "Name", 0);

            return View(cr);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public FileResult CompanyReport(int companyId)
        {

            byte[] b;
            DateTime curr = DateTime.Now.ToLocalTime();

            string currDate = $"{curr.ToShortDateString()} {curr.ToShortTimeString()}";

            DCTSOpenXML oxl = new DCTSOpenXML();
            List<Memo> memos = db.Memos.ToList();
            
            using (MemoryStream memStream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(memStream, SpreadsheetDocumentType.Workbook))
                {

                    // Build Excel File
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    // Stylesheet
                    Stylesheet stylesheet = oxl.CreateStyleSheet();
                    WorkbookStylesPart workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new DocumentFormat.OpenXml.Spreadsheet.Sheets());
                    workbookStylesPart.Stylesheet = stylesheet;
                    workbookStylesPart.Stylesheet.Save();
                    List<string> pagelist = new List<string>() { "Styles" , "Castings", "Stones", "Findings", "Labors", "Labor Items" };
                    Dictionary<string, BuildSheet> funcList = new Dictionary<string, BuildSheet>();

                    funcList.Add(pagelist[0], BuildStyleSheet);
                    funcList.Add(pagelist[1], BuildCastingsSheet);
                    funcList.Add(pagelist[2], BuildStonesSheet);
                    funcList.Add(pagelist[3], BuildFindingsSheet);
                    funcList.Add(pagelist[4], BuildLaborsSheet);
                    funcList.Add(pagelist[5], BuildLaborItemsSheet);                   

                    // declare locals
                    uint shNo = 1;
                    //for each part of the style
                    foreach (string view in pagelist)
                    {
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet(new SheetData());

                        Sheet sheet = new Sheet()
                        {
                            Id = workbookPart.GetIdOfPart(worksheetPart),
                            SheetId = shNo++,
                            Name = view
                        };
                        sheets.Append(sheet);
                        //columns = new Columns();
                        Columns columns = new Columns();
                        Worksheet worksheet;// = new Worksheet();
                        worksheet = funcList[view](db, oxl, columns, companyId);
                        worksheetPart.Worksheet = worksheet;
                    }

                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    string compName = db.FindCompany(companyId).Name;
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"{compName} Data as of {currDate}.xlsx");
                }
            }
        }

        public Worksheet BuildStyleSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId)
        {
            // do the query
            List<Style> styles = db.Styles.Include("Collection").Where(sty => sty.Collection.CompanyId == companyId).OrderBy(s => s.StyleName).ThenBy(s => s.Desc).ToList();
            // Build sheet
            // Titles            

            Worksheet worksheet = new Worksheet();
            SheetData sd = new SheetData();

            Row row;
            Cell cell;
            row = new Row();
   
            // Headers
            uint c = 1;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "StyleNum", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Style", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Desc", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E1", "Jewelry Type", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F1", "MetalWeight", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("G1", "MetalWtUnit", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "IntroDate", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("I1", "Width", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("J1", "Length", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("K1", "ChainLength", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("L1", "RetailRatio", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("M1", "RedlineRatio", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("N1", "Quantity", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("O1", "RetailPrice", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("P1", "UnitsSold", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("Q1", "Image", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("R1", "MetalWtNote", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("S1", "Collection Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("T1", "Notes", true, 1); row.Append(cell); c++;

            worksheet.Append(columns);
            oxl.columns = columns;
            sd.Append(row);

            // Content
            int rr;
            string loc;
            for (int i = 0; i < styles.Count(); i++)
            {
                row = new Row();
                rr = i + 2;
                //cell = oxl.SetCellVal("A2", "ABCEDFGHIJKLMNOP"); row.Append(cell);
                loc = "A" + rr; cell = oxl.SetCellVal(loc, styles[i].Id); row.Append(cell);
                loc = "B" + rr; cell = oxl.SetCellVal(loc, styles[i].StyleNum); row.Append(cell);
                loc = "C" + rr; cell = oxl.SetCellVal(loc, styles[i].StyleName); row.Append(cell);
                loc = "D" + rr; cell = oxl.SetCellVal(loc, styles[i].Desc); row.Append(cell);
                loc = "E" + rr; cell = oxl.SetCellVal(loc, styles[i].JewelryType.Name); row.Append(cell);
                loc = "F" + rr; cell = oxl.SetCellVal(loc, styles[i].MetalWeight?.ToString()); row.Append(cell);
                loc = "G" + rr; cell = oxl.SetCellVal(loc, styles[i].MetalWeightUnit?.Unit); row.Append(cell);
                loc = "H" + rr; cell = oxl.SetCellVal(loc, styles[i].IntroDate?.ToString() ); row.Append(cell);
                loc = "I" + rr; cell = oxl.SetCellVal(loc, styles[i].Width); row.Append(cell);
                loc = "J" + rr; cell = oxl.SetCellVal(loc, styles[i].Length); row.Append(cell);
                loc = "K" + rr; cell = oxl.SetCellVal(loc, styles[i].ChainLength); row.Append(cell);
                loc = "L" + rr; cell = oxl.SetCellVal(loc, styles[i].RetailRatio?.ToString()); row.Append(cell);
                loc = "M" + rr; cell = oxl.SetCellVal(loc, styles[i].RedlineRatio?.ToString()); row.Append(cell);
                loc = "N" + rr; cell = oxl.SetCellVal(loc, styles[i].Quantity); row.Append(cell);
                loc = "O" + rr; cell = oxl.SetCellVal(loc, styles[i].RetailPrice?.ToString()); row.Append(cell);
                loc = "P" + rr; cell = oxl.SetCellVal(loc, styles[i].UnitsSold); row.Append(cell);
                loc = "Q" + rr; cell = oxl.SetCellVal(loc, styles[i].Image); row.Append(cell);
                loc = "R" + rr; cell = oxl.SetCellVal(loc, styles[i].MetalWtNote); row.Append(cell);
                loc = "S" + rr; cell = oxl.SetCellVal(loc, styles[i].Collection.Name); row.Append(cell);
                loc = "T" + rr; cell = oxl.SetCellVal(loc, styles[i].Collection.Notes); row.Append(cell);
                sd.Append(row);
            }           
            worksheet.Append(sd);

            return worksheet;
        }

        private Worksheet BuildCastingsSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId)
        {
            // do the query
            List<MetalCode> metals = db.MetalCodes.Where(mc=> mc.CompanyId == companyId).ToList();
            List<MetalWeightUnit> mcUnits = db.MetalWeightUnits.ToList();
            List<CastingReport> cr = db.StyleCastings
                .Join(db.Castings, sc => sc.CastingId, cas => cas.Id, (sc, cas) => new CastingReport
                {
                    style = sc.Style,
                    casting = cas
                })
                .Where(r => r.style.Collection.CompanyId == companyId)
                .OrderBy(sc => sc.style.StyleName).ToList();
            // Build sheet
            // Titles            

            Worksheet worksheet = new Worksheet();
            SheetData sd = new SheetData();

            Row row;
            Cell cell;
            row = new Row();

            // Headers
            uint c = 1;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Style Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Style Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Casting Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E1", "Metal", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F1", "Weight", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("G1", "Weight Unit", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "Labor", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("I1", "Qty", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("J1", "Vendor", true, 1); row.Append(cell); c++;
            //columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "Price - computed", true, 1); row.Append(cell); c++;
            worksheet.Append(columns);
            oxl.columns = columns;
            sd.Append(row);

            // Content
            int rr;
            string loc;
            for (int i = 0; i < cr.Count(); i++)
            {
                row = new Row();
                rr = i + 2;
                loc = "A" + rr; cell = oxl.SetCellVal(loc, cr[i].style.Id); row.Append(cell);
                loc = "B" + rr; cell = oxl.SetCellVal(loc, cr[i].style.StyleName); row.Append(cell);
                loc = "C" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.Id); row.Append(cell);
                loc = "D" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.Name); row.Append(cell);
                loc = "E" + rr; cell = oxl.SetCellVal(loc, db.MetalCodes.Find(cr[i].casting.MetalCodeID.GetValueOrDefault()).Code); row.Append(cell);
                loc = "F" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.MetalWeight); row.Append(cell);
                loc = "G" + rr; cell = oxl.SetCellVal(loc, db.MetalWeightUnits.Find(cr[i].casting.MetalWtUnitId).Unit); row.Append(cell);
                loc = "H" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.Labor.GetValueOrDefault()); row.Append(cell);
                loc = "I" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.Qty.GetValueOrDefault()); row.Append(cell);
                loc = "J" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.Vendor.Name); row.Append(cell);
                //loc = "H" + rr; cell = oxl.SetCellVal(loc, cr[i].casting.Price.GetValueOrDefault()); row.Append(cell);
                //loc = "" + rr; cell = oxl.SetCellVal(loc, [i].); row.Append(cell);
                sd.Append(row);
            }
            worksheet.Append(sd);

            return worksheet;
        }

        private Worksheet BuildStonesSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId)
        {
            // do the query
            List<StoneReport> sr = db.StyleStones
                .Join(db.Stones, ss => ss.StoneId, sto => sto.Id, (ss, sto) => new StoneReport
                {
                    stone = sto,
                    style = ss.Style,
                    qty = ss.Qty
                })
                .Where(r => r.stone.CompanyId == companyId)
                .OrderBy(ss => ss.style.StyleName).ToList();
            // Build sheet
            // Titles            

            Worksheet worksheet = new Worksheet();
            SheetData sd = new SheetData();

            Row row;
            Cell cell;
            row = new Row();

            // Headers
            uint c = 1;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Style Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Style Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Stone Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E1", "Shape", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F1", "Size", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("G1", "Weight", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "Vendor", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("I1", "Price", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("J1", "Qty", true, 1); row.Append(cell); c++;
            worksheet.Append(columns);
            oxl.columns = columns;
            sd.Append(row);

            // Content
            int rr;
            string loc;
            for (int i = 0; i < sr.Count(); i++)
            {
                row = new Row();
                rr = i + 2;
                loc = "A" + rr; cell = oxl.SetCellVal(loc, sr[i].style.Id); row.Append(cell);
                loc = "B" + rr; cell = oxl.SetCellVal(loc, sr[i].style.StyleName); row.Append(cell);
                loc = "C" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.Id); row.Append(cell);
                loc = "D" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.Name); row.Append(cell);
                loc = "E" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.Shape.Name); row.Append(cell);
                loc = "F" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.StoneSize); row.Append(cell);
                loc = "G" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.CtWt.GetValueOrDefault()); row.Append(cell);
                loc = "H" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.Vendor.Name); row.Append(cell);
                loc = "I" + rr; cell = oxl.SetCellVal(loc, sr[i].stone.Price); row.Append(cell);
                loc = "J" + rr; cell = oxl.SetCellVal(loc, sr[i].qty.GetValueOrDefault()); row.Append(cell);
                sd.Append(row);
            }
            worksheet.Append(sd);

            return worksheet;
        }

        private Worksheet BuildFindingsSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId)
        {
            // do the query
            List<FindingReport> fr = db.StyleFindings
                .Join(db.Findings, sf => sf.FindingId, fin => fin.Id, (sf, fin) => new FindingReport
                {
                    finding = fin,
                    style = sf.Style,
                    qty = sf.Qty
                })
                .Where(r => r.finding.CompanyId == companyId)
                .OrderBy(sf => sf.style.StyleName).ToList();
            // Build sheet
            // Titles            

            Worksheet worksheet = new Worksheet();
            SheetData sd = new SheetData();

            Row row;
            Cell cell;
            row = new Row();

            // Headers
            uint c = 1;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Style Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Style Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Finding Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E1", "Weight", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F1", "Vendor", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("G1", "Price", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "Qty", true, 1); row.Append(cell); c++;
            worksheet.Append(columns);
            oxl.columns = columns;
            sd.Append(row);

            // Content
            int rr;
            string loc;
            for (int i = 0; i < fr.Count(); i++)
            {
                row = new Row();
                rr = i + 2;
                loc = "A" + rr; cell = oxl.SetCellVal(loc, fr[i].style.Id); row.Append(cell);
                loc = "B" + rr; cell = oxl.SetCellVal(loc, fr[i].style.StyleName); row.Append(cell);
                loc = "C" + rr; cell = oxl.SetCellVal(loc, fr[i].finding.Id); row.Append(cell);
                loc = "D" + rr; cell = oxl.SetCellVal(loc, fr[i].finding.Name); row.Append(cell);
                loc = "E" + rr; cell = oxl.SetCellVal(loc, fr[i].finding.Weight.GetValueOrDefault()); row.Append(cell);
                loc = "F" + rr; cell = oxl.SetCellVal(loc, fr[i].finding.Vendor.Name); row.Append(cell);
                loc = "G" + rr; cell = oxl.SetCellVal(loc, fr[i].finding.Price); row.Append(cell);
                loc = "H" + rr; cell = oxl.SetCellVal(loc, fr[i].qty.GetValueOrDefault()); row.Append(cell);
                sd.Append(row);
            }
            worksheet.Append(sd);

            return worksheet;
        }

        private Worksheet BuildLaborsSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId)
        {
            {
                // do the query
                List<LaborReport> lr = db.StyleLabors.Join(db.Labors, sl => sl.LaborId, lab => lab.Id, (sl, lab) => new LaborReport 
                {
                    labor = lab, 
                    style = sl.Style
                })
                .Where(r => r.style.Collection.CompanyId == companyId)
                .OrderBy(sl => sl.style.StyleName).ToList();
                // Build sheet
                // Titles            

                Worksheet worksheet = new Worksheet();
                SheetData sd = new SheetData();

                Row row;
                Cell cell;
                row = new Row();

                // Headers
                uint c = 1;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Style Id", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Style Name", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Labor Id", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Name", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E1", "Vendor", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F1", "Desc", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("G1", "$/H", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "$/P", true, 1); row.Append(cell); c++;
                columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("I1", "Qty", true, 1); row.Append(cell); c++;
                worksheet.Append(columns);
                oxl.columns = columns;
                sd.Append(row);

                // Content
                int rr;
                string loc;
                for (int i = 0; i < lr.Count(); i++)
                {
                    row = new Row();
                    rr = i + 2;
                    loc = "A" + rr; cell = oxl.SetCellVal(loc, lr[i].style.Id); row.Append(cell);
                    loc = "B" + rr; cell = oxl.SetCellVal(loc, lr[i].style.StyleName); row.Append(cell);
                    loc = "C" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.Id); row.Append(cell);
                    loc = "D" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.Name); row.Append(cell);
                    loc = "E" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.Vendor?.Name); row.Append(cell);
                    loc = "F" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.Desc); row.Append(cell);
                    loc = "G" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.PricePerHour.GetValueOrDefault()); row.Append(cell);
                    loc = "H" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.PricePerPiece.GetValueOrDefault()); row.Append(cell);
                    loc = "I" + rr; cell = oxl.SetCellVal(loc, lr[i].labor.Qty.GetValueOrDefault()); row.Append(cell);
                    sd.Append(row);
                }
                worksheet.Append(sd);

                return worksheet;
            }
        }

        private Worksheet BuildLaborItemsSheet(OJewelryDB db, DCTSOpenXML oxl, Columns columns, int companyId)
        {
            // do the query
            List<StyleLaborTableItem> lll = db.StyleLaborItems.ToList();
            List<LaborTableReport> ltr = db.StyleLaborItems
                .Join(db.LaborTable, sli => sli.LaborTableId, lab => lab.Id, (sli, lab) => new LaborTableReport
                {
                    laborItem = lab,
                    style = sli.Style,
                    qty = sli.Qty
                })
                .Where(r => r.laborItem.CompanyId == companyId)
                .OrderBy(sli => sli.style.StyleName).ToList();// Build sheet
            // Titles            

            Worksheet worksheet = new Worksheet();
            SheetData sd = new SheetData();

            Row row;
            Cell cell;
            row = new Row();

            // Headers
            uint c = 1;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("A1", "Style Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("B1", "Style Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("C1", "Labor Table Id", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("D1", "Name", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("E1", "$/P", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("F1", "$/H", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("G1", "Vendor", true, 1); row.Append(cell); c++;
            columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = c, Max = c, BestFit = true, CustomWidth = true }); cell = oxl.SetCellVal("H1", "Qty", true, 1); row.Append(cell); c++;
            worksheet.Append(columns);
            oxl.columns = columns;
            sd.Append(row);

            // Content
            int rr;
            string loc;
            for (int i = 0; i < ltr.Count(); i++)
            {
                row = new Row();
                rr = i + 2;
                loc = "A" + rr; cell = oxl.SetCellVal(loc, ltr[i].style.Id); row.Append(cell);
                loc = "B" + rr; cell = oxl.SetCellVal(loc, ltr[i].style.StyleName); row.Append(cell);
                loc = "C" + rr; cell = oxl.SetCellVal(loc, ltr[i].laborItem.Id); row.Append(cell);
                loc = "D" + rr; cell = oxl.SetCellVal(loc, ltr[i].laborItem.Name); row.Append(cell);
                loc = "E" + rr; cell = oxl.SetCellVal(loc, ltr[i].laborItem.ppp.GetValueOrDefault()); row.Append(cell);
                loc = "F" + rr; cell = oxl.SetCellVal(loc, ltr[i].laborItem.pph.GetValueOrDefault()); row.Append(cell);
                loc = "G" + rr; cell = oxl.SetCellVal(loc, ltr[i].laborItem.Vendor?.Name); row.Append(cell);
                loc = "H" + rr; cell = oxl.SetCellVal(loc, ltr[i].qty); row.Append(cell);
                //loc = "F" + rr; cell = oxl.SetCellVal(loc, laborItems[i]); row.Append(cell);
                sd.Append(row);
            }
            worksheet.Append(sd);

            return worksheet;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult FixFinishingCost()
        {
            List<Style> WithOutFC = GetStylesWithoutFinishingCost();
            return View(WithOutFC);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult FixFinishingConfirm()
        {
            List<Style> WithOutFC = GetStylesWithoutFinishingCost();
            int lid = -1;
            foreach (Style s in WithOutFC)
            {
                // Create a Labor and a StyleLabor
                Labor l = new Labor() {
                    Id = lid,
                    Name = "FINISHING LABOR",
                    PricePerPiece = 0,
                    PricePerHour = 0,
                    Qty =1
                };
                StyleLabor sl = new StyleLabor()
                {
                    StyleId = s.Id,
                    LaborId = lid                    
                };
                db.Labors.Add(l);
                db.StyleLabors.Add(sl);
                lid--;
            }
            db.SaveChanges();
            return RedirectToAction("FixFinishingCost");
        }

        private List<Style> GetStylesWithoutFinishingCost()
        {
            List<Style> AllStyles = db.Styles.ToList();
            List<Style> WithFC = db.StyleLabors.Where(sl => sl.Labor.Name == "FINISHING LABOR").Select(sl => sl.Style).ToList();
            List<Style> WithOutFC = AllStyles.Except(WithFC).ToList();

            return WithOutFC;
        }
    }
}
