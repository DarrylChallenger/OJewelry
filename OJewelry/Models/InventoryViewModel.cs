using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Models
{
    public class InventoryViewModel
    {
        public InventoryViewModel()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
            bCC_CompCollCreated = false;
        }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        //public int CompanyQty { get; set; } 
        public SelectList FromLocations { get; set; }
        public SelectList ToLocations { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public List<String> Errors { get; set; }
        public List<String> Warnings { get; set; }
        public bool bCC_CompCollCreated { get; set; }

        [ExcelAttachment]
        public HttpPostedFileBase AddPostedFile { get; set; }

        [ExcelAttachment]
        public HttpPostedFileBase MovePostedFile { get; set; }

    }

    public class InventoryReportModel
    {
        public List<irmStyle> styles { get; set; }
        public List<irmLocation> locations { get; set; }
        public List<irmLS> locationQuantsbystyle { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }

    public class irmStyle
    {
        public int StyleId { get; set; }
        //public int PresenterId { get; set; }
        public string StyleNum { get; set; }
        public string StyleName { get; set; }
        public string StyleDesc { get; set; }
        public int StyleQuantity { get; set; }
        public decimal StylePrice { get; set; }
        public int StyleQtySold { get; set; }
        public string StyleCollectionName { get; set; }
    }

    public class irmLocation
    {
        public int PresenterId { get; set; }
        public string PresenterName { get; set; }
        private string shortname;
        public string ShortName
        {
            get
            {
                if (shortname == null || shortname == "")
                { return PresenterName; }
                else { return shortname; }
            }
            set
            {
                shortname = value;
            }
        }
    }

    public class irmLS
    {
        public int StyleId { get; set; }
        public int PresenterId { get; set; }
        public string StyleNum { get; set; }
        public int StyleQuantity { get; set; }
        public string LocationName { get; set; }
        public int MemoQty { get; set; }
    }
}


[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelAttachmentAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value,
      ValidationContext validationContext)
    {
        HttpPostedFileBase file = value as HttpPostedFileBase;

        // The file is required.
        if (file == null)
        {
            return new ValidationResult("Please upload a file!");//, new [] { validationContext.ToString() });
        }

        // The meximum allowed file size is 10MB.
        if (file.ContentLength > 10 * 1024 * 1024)
        {
            return new ValidationResult("This file is too big!");
        }

        // Only PDF can be uploaded.
        string ext = Path.GetExtension(file.FileName);
        if (String.IsNullOrEmpty(ext) || 
            (!ext.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) &&
           !ext.Equals(".xls", StringComparison.OrdinalIgnoreCase)))
        {
            return new ValidationResult("This file is not an Excel file!");
        }

        // Everything OK.
        return ValidationResult.Success;
    }
}