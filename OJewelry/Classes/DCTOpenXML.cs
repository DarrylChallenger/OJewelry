using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using System.IO.Packaging;
using System.IO;

namespace OJewelry.Classes
{
    public class DCTSOpenXML
    {
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

        public Cell SetCellVal(string loc, int val)
        {

            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            return cell;
        }

        public Cell SetCellVal(string loc, decimal val)
        {

            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            return cell;
        }

        public Cell SetCellVal(string loc, string val)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.String, CellValue = new CellValue(val) };
            return cell;
        }

        public Cell SetCellVal(string loc, float val)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            return cell;
        }

        public Cell SetCellVal(string loc, double val)
        {
            Cell cell = new Cell() { CellReference = loc, DataType = CellValues.Number, CellValue = new CellValue(val.ToString()) };
            return cell;
        }

    }
}