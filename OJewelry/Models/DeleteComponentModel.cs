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
            castings = new List<Casting>();
            stones = new List<Stone>();
            findings = new List<Finding>();
            labors = new List<Labor>();
            laborItems = new List<LaborItem>();
            bError = false;
        }
        public bool bError { get; set; }
        public Vendor item { get; set; }
        public List<Casting> castings { get; set; }
        public List<Stone> stones { get; set; }
        public List<Finding> findings { get; set; }
        public List<Labor> labors { get; set; }
        public List<LaborItem> laborItems { get; set; }
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

    public class CastingEqualityComparer : IEqualityComparer<Casting>
    {
        public bool Equals(Casting x, Casting y)
        {
            if (Object.ReferenceEquals(x, null)) return false;
            if (Object.ReferenceEquals(this, y)) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(Casting c)
        {
            return c.Id.GetHashCode();
        }
    }

    public class StoneEqualityComparer : IEqualityComparer<Stone>
    {
        public bool Equals(Stone x, Stone y)
        {
            if (Object.ReferenceEquals(x, null)) return false;
            if (Object.ReferenceEquals(this, y)) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(Stone c)
        {
            return c.Id.GetHashCode();
        }
    }

    public class FindingEqualityComparer : IEqualityComparer<Finding>
    {
        public bool Equals(Finding x, Finding y)
        {
            if (Object.ReferenceEquals(x, null)) return false;
            if (Object.ReferenceEquals(this, y)) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(Finding f)
        {
            return f.Id.GetHashCode();
        }
    }

    public class LaborEqualityComparer : IEqualityComparer<Labor>
    {
        public bool Equals(Labor x, Labor y)
        {
            if (Object.ReferenceEquals(x, null)) return false;
            if (Object.ReferenceEquals(this, y)) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(Labor l)
        {
            return l.Id.GetHashCode();
        }
    }

    public class LaborItemEqualityComparer : IEqualityComparer<LaborItem>
    {
        public bool Equals(LaborItem x, LaborItem y)
        {
            if (Object.ReferenceEquals(x, null)) return false;
            if (Object.ReferenceEquals(this, y)) return true;
            return x.Id == y.Id;
        }

        public int GetHashCode(LaborItem li)
        {
            return li.Id.GetHashCode();
        }
    }



}
