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

namespace OJewelry.Classes
{
    public class DCTSOpenXML
    {
        public DCTSOpenXML()
        {
            columns = new Columns();
            graphics = Graphics.FromImage(new Bitmap(100, 100));
            font = new System.Drawing.Font("Calabri", 11);
            minWidth = graphics.MeasureString("WWWWW", font).Width;
            cellBuf = "WW";
        }
        public double minWidth { get; set; }
        public Columns columns { get; set; }
        private Graphics graphics;
        private string cellBuf;

        private System.Drawing.Font font;

        public bool CellMatches(string cellref, Worksheet w, SharedStringTablePart strings, string value)
        {
            Cell cell = w.Descendants<Cell>().Where(c => c.CellReference == cellref).First();
            try
            {
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        if (strings.SharedStringTable.ElementAt(int.Parse(cell.CellValue.InnerText)).InnerText == value)
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
            catch
            {
                str = "";
            }
            return str;
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

        public Cell SetCellVal(string loc, int val, bool bSetCellWidth = true)
        {

            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, decimal val, bool bSetCellWidth = true)
        {

            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, string val, bool bSetCellWidth = true)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.String, CellValue = new CellValue(val) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val);
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, float val, bool bSetCellWidth = true)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, double val, bool bSetCellWidth = true)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, val.ToString());
                //columns.ElementAt(); // this is how to index the columns
            }
            return cell;
        }

        public Cell SetCellVal(string loc, Image img, double cellHeight, bool bSetCellWidth = true)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue("") };
            if (bSetCellWidth)
            {
                SetColumnWidth(loc, (double)ComputeExcelCellWidth(img.Width*cellHeight/img.Height));
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
                double width = graphics.MeasureString(val + cellBuf, font).Width;
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
    }
}