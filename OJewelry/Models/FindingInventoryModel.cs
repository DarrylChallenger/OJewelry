using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class FindingInventoryModel
    {
        public FindingInventoryModel()
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

    public class FindingElement
    {
        public string finding;

        public string vendorName;
        public int delta;
        public int lineNum;
    }

    public partial class Finding
    {
        public bool VendorMatchesData(string vendorName)
        {
            // If the stone or finding has a non-blank vendor name, it must match the name in the sheet
            if (Vendor != null && Vendor.Name != "" && Vendor.Name != vendorName)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public class FindingVendorMatcher
    {
        public Vendor vendor { get; set; }
        public string excelVendorName { get; set; }
    }
}