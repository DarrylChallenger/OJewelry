using System;
using System.Collections.Generic;
using System.Linq;
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
        [ExcelAttachment]
        public HttpPostedFileBase PostedFile { get; set; }
    }

    public class StoneElement
    {
        public string stone;
        public string shape;
        public string size;
        public int delta;
        public int lineNum;
    }
}