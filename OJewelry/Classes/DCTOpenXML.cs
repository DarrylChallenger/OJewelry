using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using Color = DocumentFormat.OpenXml.Spreadsheet.Color;

namespace OJewelry.Classes
{
    public class DCTSOpenXML
    {
        public DCTSOpenXML()
        {
            columns = new Columns();
            graphics = Graphics.FromImage(new Bitmap(100, 100));
            font = new System.Drawing.Font("Calabri", 11);
            minWidth = ComputeExcelCellWidth(graphics.MeasureString("WW", font).Width);
            cellBuf = "WW";
        }
        public double minWidth { get; set; }
        public Columns columns { get; set; }
        private Graphics graphics;
        private string cellBuf;

        private System.Drawing.Font font;

        public Stylesheet CreateStyleSheet()
        {
            //
            Fonts fonts = new Fonts(
                new DocumentFormat.OpenXml.Spreadsheet.Font(                                            // Index 0 - The default font.
                    new FontSize() { Val = 11 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Calibri" }),
                new DocumentFormat.OpenXml.Spreadsheet.Font(                                            // Index 1 - The bold font.
                    new FontSize() { Val = 14 },
                    new Color() { Rgb = new HexBinaryValue() { Value = "000000" } },
                    new FontName() { Val = "Calibri" })
                );
            Fills fills = new Fills(new Fill());
            Borders borders = new Borders(new Border());
            CellFormats cf = new CellFormats(
                new CellFormat() { FontId = 0, FillId = 0, BorderId = 0 },                                                                      // 0
                new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true },                                                    // 1
                new CellFormat() { FontId = 1, FillId = 0, BorderId = 0, ApplyFont = true, Alignment = new Alignment() { WrapText = true } },   // 2
                new CellFormat(new Alignment() { WrapText = true })                                                                             // 3    
                );
            Stylesheet ss = new Stylesheet(fonts, fills, borders, cf);
            return ss;
        }

        public bool CellMatches(string cellref, Worksheet w, SharedStringTablePart strings, string value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        if (strings.SharedStringTable.ElementAt(int.Parse(cell.CellValue.InnerText)).InnerText?.Trim() == value?.Trim())
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool CellMatches(string cellref, Worksheet w, int value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType == null)
                {
                    if (int.Parse(cell.CellValue.InnerText) == value)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool CellMatches(string cellref, Worksheet w, DateTime value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.Date)
                    {
                        if ((DateTime.Parse(cell.CellValue.InnerText)) == value)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public bool CellMatches(string cellref, Worksheet w, bool value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.Boolean)
                    {
                        if (Boolean.Parse(cell.CellValue.InnerText) == value)
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        public string GetStringVal(Cell cell, SharedStringTablePart strings)
        {
            String str = "";
            try
            {
                if (cell.DataType == null)
                {
                    str = cell.CellValue.InnerText;
                    return str?.Trim();
                }
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        str = strings.SharedStringTable.ElementAt(int.Parse(cell.CellValue.InnerText)).InnerText;
                    }
                }
                if (cell.DataType == CellValues.String)
                {
                    str = cell.CellValue.Text;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError($"OJException occurred {e.Message}");
                str = "";
            }
            return str?.Trim();
        }

        public int GetIntVal(Cell cell)
        {
            int i = 0;
            try
            {
                if (cell.DataType == null)
                {
                    i = int.Parse(cell.CellValue.InnerText);
                }
            }
            catch
            {
                return 0;
            }

            return i;
        }

        public double GetDoubleVal(Cell cell)
        {
            double d = 0;
            try
            {
                if (cell.DataType == null)
                {
                    d = double.Parse(cell.CellValue.InnerText);
                }
            }
            catch
            {
                return 0;
            }

            return d;
        }

        public Cell SetCellVal(string loc, int val, bool bSetCellWidth = true, uint style = 0)
        {

            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, bool val, bool bSetCellWidth = true, uint style = 0)
        {

            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.String, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, decimal val, bool bSetCellWidth = true, uint style = 0)
        {

            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, string value, bool bSetCellWidth = true, uint style = 0)
        {
            string val = value;
            if (value != null)
            {
                val = value?.Trim();
            }
            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.String, CellValue = new CellValue(val) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val);
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, float val, bool bSetCellWidth = true, uint style = 0)
        {
            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, double val, bool bSetCellWidth = true, uint style = 0)
        {
            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, Image img, double cellHeight, bool bSetCellWidth = true, uint style = 0)
        {
            Cell cell = new Cell() { CellReference = loc, StyleIndex = style, DataType = CellValues.Number, CellValue = new CellValue("") };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, (double)ComputeExcelCellWidthForImage(img.Width*cellHeight/img.Height));
            }
            return cell;
        }

        private void SetColumnWidth(string loc, string val)
        {
            int? col = null;
            // get the column out of loc and convert to an index
            if (Char.IsLetter(loc, 0) && Char.IsLetter(loc, 1))
            {
                col = (Convert.ToChar(loc[0]) - 65) * 26 + (Convert.ToChar(loc[0]) - 65);
            }
            else if (Char.IsLetter(loc, 0))
            {
                col = Convert.ToChar(loc[0]) - 65;
            }
            // set width to max(currWidth, width(val))
            if (col == null || columns.Count() <= col)
            {
                return;
            }
            else
            {
                Column c = columns.ChildElements[col.Value] as Column;
                double width = graphics.MeasureString(val?.Trim() + cellBuf, font).Width;
                // column widths are stored in points!
                c.Width = Math.Max(ComputeExcelCellWidth(width), c.Width);
            }
        }
        private void SetColumnWidth(string loc, double width)
        {
            int? col = null;
            // get the column out of loc and convert to an index
            if (Char.IsLetter(loc, 0) && Char.IsLetter(loc, 1))
            {
                col = (Convert.ToChar(loc[0]) - 65) * 26 + (Convert.ToChar(loc[0]) - 65);
            }
            else if (Char.IsLetter(loc, 0))
            {
                col = Convert.ToChar(loc[0]) - 65;
            }
            // set width to max(currWidth, width(val))
            if (col == null || columns.Count() <= col)
            {
                return;
            }
            else
            {
                Column c = columns.ChildElements[col.Value] as Column;
                c.Width = Math.Max(c.Width, width);
            }
        }

        public DoubleValue ComputeExcelCellWidth(double widthInPixel)
        {
            DoubleValue result = widthInPixel;
            if (widthInPixel > 12)
            {
                result = 1;
                result += (widthInPixel - 12) / 7;
            }
            else
                result = 1;

            return result;
        }

        public DoubleValue ComputeExcelCellWidthForImage(double widthInPixel)
        {
            DoubleValue result = 0;
            if (widthInPixel > 12)
            {
                result = 1;
                result += (widthInPixel - 12) / 7;
            }
            else
                result = 1;

            return result;
        }

        public DoubleValue ComputeExcelCellHeight(double heightInPixel)
        {
            return (float)heightInPixel * 72 / 96;
        }

        public string GetCellName(UInt32 col, int row)
        {
            string cellName = "";
            if (col < 26)
            {
                cellName = (char)(64 + col) + row.ToString();              
            } else {
                cellName = (char)(64 + col / 26) + (char)(64 + col % 26) + row.ToString();
            }
            return cellName;
        }
    }
}