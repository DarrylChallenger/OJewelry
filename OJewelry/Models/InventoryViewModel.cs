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
        }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public SelectList FromLocations { get; set; }
        public SelectList ToLocations { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public List<String> Errors { get; set; }

        [Required(ErrorMessage ="Please choose a sheet.")]
        [ExcelAttachment]
        public HttpPostedFileBase AddPostedFile { get; set; }
        public HttpPostedFileBase MovePostedFile { get; set; }


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
            return new ValidationResult("Please upload a file!");
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