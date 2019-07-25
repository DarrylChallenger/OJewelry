using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public partial class Finding
    {
        public Finding(Finding f)
        {
            CompanyId = f.CompanyId;
            VendorId = f.VendorId;
            Name = f.Name;
            Desc = f.Desc;
            Price = f.Price;
            Weight = f.Weight;
            Qty = f.Qty;
            Note = f.Note;
        }
    }

    public partial class Stone
    {
        public Stone(Stone s)
        {
            CompanyId = s.CompanyId;
            VendorId = s.VendorId;
            Name = s.Name;
            Label = s.Label;
            Desc = s.Desc;
            CtWt = s.CtWt;
            StoneSize = s.StoneSize;
            ShapeId = s.ShapeId;
            Price = s.Price;
            SettingCost = s.SettingCost;
            Qty = s.Qty;
            Note = s.Note;
            ParentHandle = s.ParentHandle;
            Title = s.Title;
            Tags = s.Tags;
        }
    }

}
