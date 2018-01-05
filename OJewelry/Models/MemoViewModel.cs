﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Models
{
    /*
     public class PresenterModelx
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
    }
    */
    public class MemoModel
    {
        public int Id { get; set; }
        public String PresenterName  { get; set; }
        public String PresenterPhone { get; set; }
        public String PresenterEmail { get; set; }
        public DateTime? date { get; set; }
        public int Quantity { get; set; }
        public String Notes { get; set; }
        [Display(Name = "Return Qty")] public int RetrunQty { get; set; }

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
        public int PresenterId { get; set; }
        public int numPresentersWithStyle { get; set; }
        public int CompanyId { get; set; }
    }
    
    public class MemoViewModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            MemoViewModel m = (MemoViewModel)base.BindModel(controllerContext, bindingContext);
            var request = controllerContext.HttpContext.Request;
            // build memos
            m.Memos = new List<MemoModel>();
            for (int i = 0; i <m.numPresentersWithStyle; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Memos[{0}].RetrunQty", i);
                string s = request.Form.Get(sb.ToString());
                if (!Int32.TryParse(s, out int retrunQty)) retrunQty = 0;
                sb.Clear();
                sb.AppendFormat("Memos[{0}].Id", i);
                s = request.Form.Get(sb.ToString());
                if (!Int32.TryParse(s, out int id)) id = 0;
                sb.Clear();
                sb.AppendFormat("Memos[{0}].Quantity", i);
                s = request.Form.Get(sb.ToString());
                if (!Int32.TryParse(s, out int qty)) qty = 0;

                MemoModel memo = new MemoModel()
                {
                    Id = id,
                    RetrunQty = retrunQty,
                    Quantity = qty,
                };
                m.Memos.Add(memo);
            }

            return m;           
            
        }
    }
}

