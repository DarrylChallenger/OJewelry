using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class DeleteStoneModel
    {
        public DeleteStoneModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public Stone stone { get; set; }
        public List<Style> styles { get; set; }
    }

    public class DeleteFindingModel
    {
        public DeleteFindingModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public Finding finding { get; set; }
        public List<Style> styles { get; set; }
    }

}