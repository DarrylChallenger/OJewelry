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

    public class DeleteCollectionModel
    {
        public DeleteCollectionModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public Collection item { get; set; }
        public List<Style> styles { get; set; }
    }

    public class DeleteJewelryTypeModel
    {
        public DeleteJewelryTypeModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public JewelryType item { get; set; }
        public List<Style> styles { get; set; }
    }

    public class DeleteLaborItemModel
    {
        public DeleteLaborItemModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public LaborItem item { get; set; }
        public List<Style> styles { get; set; }
    }

    public class DeleteMetalCodeModel
    {
        public DeleteMetalCodeModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public MetalCode item { get; set; }
        public List<Style> styles { get; set; }
    }

    public class DeleteVendorModel
    {
        public DeleteVendorModel()
        {
            styles = new List<Style>();
            bError = false;
        }
        public bool bError { get; set; }
        public Vendor item { get; set; }
        public List<Style> styles { get; set; }
    }

    public class StyleEqualityComparer : IEqualityComparer<Style>
    {
        public bool Equals(Style x, Style y)
        {
            if (Object.ReferenceEquals(x, null)) return false;
            if (Object.ReferenceEquals(this, y)) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(Style style)
        {
            return style.Id.GetHashCode();
        }
    }

}
