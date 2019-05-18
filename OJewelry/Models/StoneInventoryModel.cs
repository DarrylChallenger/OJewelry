using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace OJewelry.Models
{
    public class StoneInventoryModel
    {
        public StoneInventoryModel()
        {
            success = false;
            Errors = new List<string>();
            Warnings = new List<string>();
        }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<String> Errors { get; set; }
        public List<String> Warnings { get; set; }
        public bool success { get; set; }
        public string failedFileName { get; set; }
        [ExcelAttachment]
        public HttpPostedFileBase PostedFile { get; set; }
    }

    public class StoneElement
    {
        public string stone;
        public string shape;
        public string size;
        public string vendorName;
        public int delta;
        public int lineNum;
    }

    // https://forums.asp.net/t/2105106.aspx?How+custom+function+can+be+used+with+LINQ+order+by+clause, https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.icomparer-1?view=netframework-4.8,https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expression-language-quick-reference
    public partial class StoneSorter : IComparer<string>
    {
        public int Compare(string size1, string size2)
        {
            // find the first number in each string and compare against each other
            string s1 = "", s2 = "";
            decimal n1 = 0, n2 = 0;
            Match m1 = Regex.Match(size1, @"^(\d*)?\.?\d+");
            Match m2 = Regex.Match(size2, @"^(\d*)?\.?\d+");
            if (m1.Success)
            {
                s1 = m1.Value;
                decimal.TryParse(s1, out n1);
            }
            if (m2.Success)
            {
                s2 = m2.Value;
                decimal.TryParse(s2, out n2);
            }
            int res = n1.CompareTo(n2);
            if (res ==0)
            {
                res = size1.CompareTo(size2);
            }
            return res;
        }
    } 
}