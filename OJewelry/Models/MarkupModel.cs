using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public enum MMState { Clean, Dirty, Added, Deleted, Unadded, Fixed }

    public class MarkupModel
    {
        public MarkupModel()
        {
            markups = new List<Markup>();
        }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public List<Markup> markups { get; set; }
    }
}