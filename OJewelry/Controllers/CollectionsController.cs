using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using A = DocumentFormat.OpenXml.Drawing;
using A14 = DocumentFormat.OpenXml.Office2010.Drawing;

using OJewelry.Classes;
using OJewelry.Models;
using System.Threading.Tasks;

namespace OJewelry.Controllers
{
    [Authorize]
    public class CollectionsController : Controller
    {
        private OJewelryDB db = new OJewelryDB();

        private int relId = 0;
        private uint imageId = 1025;
        Drawing drawing = null;
        private int pixelRowHeight = 60;

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

        public async Task<FileResult> ExportCollectionReport(int companyId)
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
                    //oxl.columns = columns;
                    //for each collection
                    foreach (Collection collection in db.Collections.Where(c => c.CompanyId == companyId).OrderBy(c => c.Name).Include("Styles"))
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
                        // Clear out the drawing object form each page
                        drawing = null;
                        //columns = new Columns();
                        Columns columns = new Columns();
                        Worksheet worksheet = new Worksheet();
                        SheetData sd = new SheetData();
                        List<Bitmap> images = new List<Bitmap>();
                        string imageName = "";
                        // Build sheet
                        // Headers
                        row = new Row();
                        cell = oxl.SetCellVal("A1", ""); row.Append(cell); columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 1, Max = 1, BestFit = true, CustomWidth = true });
                        cell = oxl.SetCellVal("B1", "Name"); row.Append(cell); columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 2, Max = 2, BestFit = true, CustomWidth = true });
                        cell = oxl.SetCellVal("C1", "Desc"); row.Append(cell); columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 3, Max = 3, BestFit = true, CustomWidth = true });
                        cell = oxl.SetCellVal("D1", "Style No."); row.Append(cell); columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 4, Max = 4, BestFit = true, CustomWidth = true });
                        cell = oxl.SetCellVal("E1", "Inventory"); row.Append(cell); columns.Append(new Column() { Width = oxl.ComputeExcelCellWidth(oxl.minWidth), Min = 5, Max = 5, BestFit = true, CustomWidth = true });
                        sd.Append(row);
                        worksheet.Append(columns);
                        oxl.columns = columns;
                        List<Style> Styles = collection.Styles.OrderBy(s => s.StyleName).ThenBy(s => s.Desc).ToList();
                        // Content
                        for (int i = 0; i < Styles.Count(); i++)
                        {
                            row = new Row();
                            rr = 2 + i;
                            Bitmap image;
                            if (Styles[i].Image == null)
                            {
                                image = new Bitmap(Server.MapPath("/Images") + "/logo.png");
                            } else
                            {
                                // get the file off storage 
                                image = new Bitmap(await Singletons.azureBlobStorage.Download(Styles[i].Image));
                                //image = Image.FromFile(Server.MapPath("/Images") + "/logo.png");
                            }
                            images.Add(image);
                            row.Height = oxl.ComputeExcelCellHeight(pixelRowHeight);
                            row.CustomHeight = true;
                            loc = "A" + rr; cell = oxl.SetCellVal(loc, image, pixelRowHeight); row.Append(cell); 
                            loc = "B" + rr; cell = oxl.SetCellVal(loc, Styles[i].StyleName); row.Append(cell);
                            loc = "C" + rr; cell = oxl.SetCellVal(loc, Styles[i].Desc); row.Append(cell);
                            loc = "D" + rr; cell = oxl.SetCellVal(loc, Styles[i].StyleNum); row.Append(cell);
                            loc = "E" + rr; cell = oxl.SetCellVal(loc, Styles[i].Quantity); row.Append(cell);
                            sd.Append(row);
                        }
                        worksheet.Append(sd);
                        worksheetPart.Worksheet = worksheet;
                        // Images
                        Column col0 = columns.ElementAt(0) as Column;
                        double col0Width = col0.Width;

                        for (int i =0; i < Styles.Count(); i++)
                        {
                            if (Styles[i].Image == null)
                            {
                                imageName = Server.MapPath("/Images") + "/logo.png";
                            }
                            else
                            {
                                // get the file off storage 
                                imageName = Styles[i].Image;
                                //imageName = Server.MapPath("/Images") + "/logo.png";

                            }
                            rr = 2 + i;
                            // place images in column A
                            string contentType = MimeMapping.GetMimeMapping(imageName);
                            PlaceImageOnCell(worksheet, images[i], 0, rr-1, col0Width, pixelRowHeight, contentType, Styles[i].StyleName, Styles[i].Desc);
                            
                        }
                    }

                    workbookPart.Workbook.Save();
                    document.Close();

                    b = memStream.ToArray();
                    string compName = db.FindCompany(companyId).Name;
                    return File(b, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        compName + " Collection as of " + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }

        private void PlaceImageOnCell(Worksheet worksheet, Bitmap image, int Col, int Row, double colWid, double rowHeight, string type, string imgName="", string imgDesc="")//, float? W, float? H)
        {
            Dictionary<string, ImagePartType> mimeToImagePartType = new Dictionary<string, ImagePartType>()
            {
                { "image/bmp", ImagePartType.Bmp },
                { "image/gif", ImagePartType.Gif },
                { "image/jpg", ImagePartType.Jpeg },
                { "image/jpeg", ImagePartType.Jpeg },
                { "image/png", ImagePartType.Png },
                { "image/tiff", ImagePartType.Tiff }           
            };
            try
            {
                ImagePart imagePart;
                ImagePartType ip;
                if (!mimeToImagePartType.TryGetValue(type, out ip))
                {
                    ip = ImagePartType.Jpeg;
                }
                WorksheetDrawing wsd;
                DrawingsPart dp;
                WorksheetPart worksheetPart = worksheet.WorksheetPart;

                double imgMargin = 5;

                //string sImagePath = "D:/Documents/Source/Repos/DarrylSite/DarrylSite/Images/DC.png";
                if (worksheetPart.DrawingsPart == null)
                {
                    dp = worksheetPart.AddNewPart<DrawingsPart>();
                    imagePart = dp.AddImagePart(ip, worksheetPart.GetIdOfPart(dp));
                    wsd = new WorksheetDrawing();
                }
                else
                {
                    dp = worksheetPart.DrawingsPart;
                    imagePart = dp.AddImagePart(ip);
                    dp.CreateRelationshipToPart(imagePart);
                    wsd = dp.WorksheetDrawing;
                }

                NonVisualDrawingProperties nvdp = new NonVisualDrawingProperties();
                nvdp.Id = GetNextImageId();
                nvdp.Name = imgName;
                nvdp.Description = imgDesc;
                DocumentFormat.OpenXml.Drawing.PictureLocks picLocks = new DocumentFormat.OpenXml.Drawing.PictureLocks();
                picLocks.NoChangeAspect = true;
                picLocks.NoChangeArrowheads = true;
                NonVisualPictureDrawingProperties nvpdp = new NonVisualPictureDrawingProperties();
                nvpdp.PictureLocks = picLocks;
                NonVisualPictureProperties nvpp = new NonVisualPictureProperties();
                nvpp.NonVisualDrawingProperties = nvdp;
                nvpp.NonVisualPictureDrawingProperties = nvpdp;

                DocumentFormat.OpenXml.Drawing.Stretch stretch = new DocumentFormat.OpenXml.Drawing.Stretch();
                stretch.FillRectangle = new DocumentFormat.OpenXml.Drawing.FillRectangle();

                BlipFill blipFill = new BlipFill();
                DocumentFormat.OpenXml.Drawing.Blip blip = new DocumentFormat.OpenXml.Drawing.Blip();
                blip.Embed = dp.GetIdOfPart(imagePart);
                blip.CompressionState = DocumentFormat.OpenXml.Drawing.BlipCompressionValues.Email;
                /*
                    string outerXml = 
                        "<a:ext uri=\"{28A0092B-C50C-407E-A947-70E740481C1C}\">" + 
                        "<a14:useLocalDpi xmlns:a14 = \"http://schemas.microsoft.com/office/drawing/2010/main\"/>" +
                        "</a:ext> ";

                    string outerXml = "uri=\"{28A0092B-C50C-407E-A947-70E740481C1C}\">";
                */
                A14.UseLocalDpi localDpi = new A14.UseLocalDpi();
                /*
                ExtensionList extLst = new ExtensionList();
                Extension extsn = new Extension(localDpi);
                extsn.Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}";
                extLst.Append(extsn);
                blip.Append(extLst);
                */
                A.BlipExtensionList blipExtLst = new A.BlipExtensionList();
                A.BlipExtension blipExt = new A.BlipExtension() { Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}" };
                localDpi.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");
                blipExt.Append(localDpi);
                blipExtLst.Append(blipExt);
                blip.Append(blipExtLst);

                blip.Append();
                blipFill.Blip = blip;
                blipFill.SourceRectangle = new DocumentFormat.OpenXml.Drawing.SourceRectangle();
                blipFill.Append(stretch);

                DocumentFormat.OpenXml.Drawing.Transform2D t2d = new DocumentFormat.OpenXml.Drawing.Transform2D();
                DocumentFormat.OpenXml.Drawing.Offset offset = new DocumentFormat.OpenXml.Drawing.Offset();
                offset.X = 0;
                offset.Y = 0;
                t2d.Offset = offset;
                //Bitmap bm = new Bitmap(sImagePath);
                //http://en.wikipedia.org/wiki/English_Metric_Unit#DrawingML
                //http://stackoverflow.com/questions/1341930/pixel-to-centimeter
                //http://stackoverflow.com/questions/139655/how-to-convert-pixels-to-points-px-to-pt-in-net-c
                DocumentFormat.OpenXml.Drawing.Extents extents = new DocumentFormat.OpenXml.Drawing.Extents();
                //extents.Cy = (long)image.Height * (long)((float)914400 / image.VerticalResolution);
                //extents.Cy = (int)(rowHeight * 72 / 96 * (100 - (2 * imgMargin)) / 100) * 12700;
                extents.Cy = (int)((rowHeight * 72 / 96 * 12700) * (((100 - (2 * imgMargin)) / 100)));
                //extents.Cx = (int)(((((colWid - 1) * 7) + 12) * 12700) * (.5 * image.Height / image.Width));
                //extents.Cx = (int)(((((colWid - 1) * 7) + 12) * 12700) * (.5 * extents.Cy / image.Width));
                extents.Cx = image.Width * extents.Cy / image.Height;
                ;
                t2d.Extents = extents;
                ShapeProperties sp = new ShapeProperties();
                sp.BlackWhiteMode = DocumentFormat.OpenXml.Drawing.BlackWhiteModeValues.Auto;
                sp.Transform2D = t2d;
                DocumentFormat.OpenXml.Drawing.PresetGeometry prstGeom = new DocumentFormat.OpenXml.Drawing.PresetGeometry();
                prstGeom.Preset = DocumentFormat.OpenXml.Drawing.ShapeTypeValues.Rectangle;
                prstGeom.AdjustValueList = new DocumentFormat.OpenXml.Drawing.AdjustValueList();
                sp.Append(prstGeom);
                sp.Append(new DocumentFormat.OpenXml.Drawing.NoFill());

                DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture picture = new DocumentFormat.OpenXml.Drawing.Spreadsheet.Picture();
                picture.NonVisualPictureProperties = nvpp;
                picture.BlipFill = blipFill;
                picture.ShapeProperties = sp;

                Xdr.FromMarker fromMarker = new Xdr.FromMarker();
                Xdr.ToMarker toMarker = new Xdr.ToMarker();
                // From
                ColumnId columnId1 = new ColumnId();
                columnId1.Text = Col.ToString();
                ColumnOffset columnOffset1 = new ColumnOffset();
                //columnOffset1.Text = "228600";
                double colloff = ((((colWid - 1) * 7) + 12) * 12700) *72/96;
                colloff -= extents.Cx;
                colloff /= 2;
                colloff = (int)colloff;
                columnOffset1.Text = colloff.ToString();
                //columnOffset1.Text = "344968";
                RowId rowId1 = new RowId();
                rowId1.Text = Row.ToString();
                RowOffset rowOffset1 = new RowOffset();
                rowOffset1.Text = ((int)((rowHeight * 72 / 96 * 12700) * (imgMargin / 100))).ToString();

                fromMarker.Append(columnId1);
                fromMarker.Append(columnOffset1);
                fromMarker.Append(rowId1);
                fromMarker.Append(rowOffset1);

                // To
                ColumnId columnId2 = new ColumnId();
                //columnId2.Text = (Col + 1).ToString();
                columnId2.Text = (Col).ToString();
                ColumnOffset columnOffset2 = new ColumnOffset();
                //columnOffset2.Text = "0";// "152381";
                columnOffset2.Text = extents.Cx.ToString();
                //columnOffset2.Text = (image.Width*1).ToString() + "px";
                RowId rowId2 = new RowId();
                rowId2.Text = (Row + 1).ToString();
                RowOffset rowOffset2 = new RowOffset();
                rowOffset2.Text = "4572000";//"152381";;

                toMarker.Append(columnId2);
                toMarker.Append(columnOffset2);
                toMarker.Append(rowId2);
                toMarker.Append(rowOffset2);

                //Position pos = new Position(); pos.X = Row; pos.Y = Col;
                Extent ext = new Extent(); ext.Cx = extents.Cx; ext.Cy = extents.Cy;

                //TwoCellAnchor anchor = new TwoCellAnchor();
                OneCellAnchor anchor = new OneCellAnchor();
                anchor.Append(fromMarker);
                anchor.Extent = ext;
                //anchor.Append(toMarker);
                anchor.Append(picture);
                anchor.Append(new ClientData());
                wsd.Append(anchor);
                {
                    MemoryStream imgStream = new MemoryStream();
                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo codec = codecs.First(c => c.MimeType == type);
                    Guid imgGuid = codec.FormatID;
                    ImageFormat imf = new ImageFormat(imgGuid);
                    float h = image.HorizontalResolution;
                    float v = image.VerticalResolution;

                    Bitmap compressedImage = new Bitmap(64, 64);//, gr);
                    Rectangle rect = new Rectangle(0, 0, 64, 64);
                    Graphics gr = Graphics.FromImage(compressedImage);
                    gr.DrawImage(image, rect);
                    compressedImage.SetResolution(48, 48);

                    compressedImage.Save(imgStream, imf);
                    gr.Dispose();
                    compressedImage.Dispose();
                    imgStream.Position = 0;
                    imagePart.FeedData(imgStream);
                    imgStream.Dispose();
                }
                if (drawing == null)
                {
                    drawing = new Drawing();
                    drawing.Id = dp.GetIdOfPart(imagePart);
                    worksheet.Append(drawing);
                }
                wsd.Save(dp);
                worksheetPart.Worksheet.Save();
                /* Sheets sheets = new Sheets();Sheet sheet = new Sheet();sheet.Name = "Sheet1";sheet.SheetId = 1;sheet.Id = wbp.GetIdOfPart(wsp);sheets.Append(sheet);wb.Append(sheets); */
            }
            catch (Exception e)
            {

            }
        }

        private string _GetNextRelationShipId()
        {
            relId++;
            return "rId" + relId.ToString();
        }

        private uint GetNextImageId()
        {
            return imageId++;
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
