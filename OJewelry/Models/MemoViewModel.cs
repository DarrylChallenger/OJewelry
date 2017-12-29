using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace OJewelry.Models
{
    public class PresenterModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
    }

    public class MemoModel
    {
        public int Id { get; set; }
        public PresenterModel presenter { get; set; }
        public DateTime date { get; set; }
        public int Quantity { get; set; }
        public String Notes { get; set; }
    }
        
    public class MemoViewModel
    {
        public StyleModel style { get; set; }
        public List<MemoModel> Memos { get; set; }
        public int SendReturnMemoRadio { get; set; } // Radio Bth
        public int NewExistingPresenterRadio { get; set; } // Radio Bth
        public List<SelectListItem> Presenters { get; set; } // dropdown
        [Display(Name="Name")] public String PresenterName { get; set; }
        [Display(Name = "Phone")] public String PresenterPhone { get; set; }
        [Display(Name = "Email")] public String PresenterEmail { get; set; }
        [Display(Name = "Send Qty")] public int SendQty { get; set; }
        [Display(Name = "Return Qty")] public int RetrunQty { get; set; }
        public int PresenterId { get; set; }
    }
}