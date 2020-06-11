using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Models
{
    public class CompanyReport
    {
        public CompanyReport()
        {
            companyId = 0;
        }

        [Required]
        [Display(Name = "Company")]
        public int companyId { get; set; }  
        
        public SelectList CompanyList { get; set; }
    }
}