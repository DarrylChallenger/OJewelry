using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public enum LMState { Clean, Dirty, Added, Deleted, Unadded, Fixed }

    public class LaborTableModel
    {
        public LaborTableModel()
        {
            Labors = new List<LaborItem>();
        }
        public List<LaborItem> Labors { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
    }
}