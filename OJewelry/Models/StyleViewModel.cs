using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Models
{
    public class MetalComponent : StyleViewComponentModel
    {
        public MetalComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public MetalComponent(Component c) { Comp = c; }
        // VENDOR	METAL	PRICE	LABOR 
        public String Vendor { get { if ((Comp.Vendor != null) && (Comp.Vendor.Name != null)) { return Comp.Vendor.Name; } else return ""; } set { if (Comp.Vendor != null) { Comp.Vendor.Name = value; } } }
        public int VendorID { get { if (Comp.Vendor != null) { return Comp.Vendor.Id; } else { return 0; } } set { if (Comp.Vendor != null) { Comp.Vendor.Id = value; } } }
        public String Metal { get { return Comp.MetalMetal ?? ""; } set { Comp.MetalMetal = value; } }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal? Price {
            get {
                return Comp.Price ?? 0;
            }
            set {
                Comp.Price = value;
            }
        }

        [Display(Name ="Labor")]
        public decimal? Labor { get { return Comp.MetalLabor ?? 0; } set { Comp.MetalLabor = value; } }
    }
    public class StoneComponent : StyleViewComponentModel
    {
        public StoneComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public StoneComponent(Component c) { Comp = c; }
        // VENDOR CT WT SIZE    PPC/$
        public String Vendor { get { if ((Comp.Vendor != null) && (Comp.Vendor.Name != null)) { return Comp.Vendor.Name; } else return ""; } set { if (Comp.Vendor != null) { Comp.Vendor.Name = value; } } }
        public int VendorID { get { if (Comp.Vendor != null) { return Comp.Vendor.Id; } else { return 0; } } set { if (Comp.Vendor != null) { Comp.Vendor.Id = value; } } }
        public int? CtWt { get { return Comp.StonesCtWt ?? 0; } set { Comp.StonesCtWt = value; } }
        public String Size { get { return Comp.StoneSize ?? ""; } set { Comp.StoneSize = value; } }

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal? PPC { get { return Comp.StonePPC ?? 0; } set { Comp.StonePPC = value; } }
    }
    public class FindingsComponent : StyleViewComponentModel
    {
        public FindingsComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public FindingsComponent(Component c) { Comp = c; }
        // VENDOR	METAL	PRICE 
        public String Vendor { get { if ((Comp.Vendor != null) && (Comp.Vendor.Name != null)) { return Comp.Vendor.Name; } else return ""; } set { if (Comp.Vendor != null) { Comp.Vendor.Name = value; } } }
        public int VendorID { get { if (Comp.Vendor != null) { return Comp.Vendor.Id; } else { return 0; } } set { if (Comp.Vendor != null) { Comp.Vendor.Id = value; } } }
        public String Metal { get { return Comp.FindingsMetal ?? ""; } set { Comp.FindingsMetal = value; } }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal? Price { get { return Comp.Price ?? 0; } set { Comp.Price = value; } }
    }
    public class LaborComponent : StyleViewComponentModel
    {
        public LaborComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public LaborComponent(Component c) { Comp = c; }
        // PRICE/HR	PRICE/PC

        [Display(Name = "$/Hour")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal? PPH { get { return Comp.PricePerHour ?? 0; } set { Comp.PricePerHour = value; } }

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal? PPP { get { return Comp.PricePerPiece ?? 0; } set { Comp.PricePerPiece = value; } }
    }
    public class MiscComponent : StyleViewComponentModel
    {
        public MiscComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public MiscComponent(Component c) { Comp = c; }
        // PRICE/PC

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal? PPP { get { return Comp.PricePerPiece ?? 0; } set { Comp.PricePerPiece = value; } }
    }

    public class StyleViewComponentModel
    {
        public StyleViewComponentModel()
        {
            SVMState = SVMStateEnum.Dirty; // For now, update all records
        }
        public void Init()
        {
            Comp = new Component();
            Comp.Id = 0;
            Comp.Price = 0;
            Comp.PricePerHour = 0;
            Comp.PricePerPiece = 0;
            Comp.MetalLabor = 0;
            Comp.StonesCtWt = 0;
            Comp.StonePPC = 0;
            // Comp.Vendor = new Vendor(); 
        }
        public int Id { get { return Comp.Id; } set { Comp.Id = value; } }
        public int CompanyId { get { return Comp.CompanyId.Value; } set { Comp.CompanyId = value; } }
        public int ComponentTypeId { get { return Comp.ComponentTypeId; } set { Comp.ComponentTypeId = value; } }
        public Component Comp { get; set; }
        public String Name { get { return Comp.Name; } set { Comp.Name = value; } }
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
        public decimal Total { get; set; }
        public SVMStateEnum SVMState { get; set; }
    }

    public enum SVMStateEnum { Clean, Dirty, Added, Deleted }

    public class StyleViewModel
    {
        public StyleViewModel() { SVMState = SVMStateEnum.Dirty; } // For now, update all records
        public Style Style { get; set; }
        public List<MetalComponent> Metals { get; set; }
        public List<StoneComponent> Stones { get; set; }
        public List<FindingsComponent> Findings { get; set; }
        public List<LaborComponent> Labors { get; set; }
        public List<MiscComponent> Miscs { get; set; }
        public decimal MetalsTotal { get; set; }
        public decimal StonesTotal { get; set; }
        public decimal FindingsTotal { get; set; }
        public decimal LaborsTotal { get; set; }
        public decimal MiscsTotal { get; set; }
        public decimal Total { get; set; }
        public SVMStateEnum SVMState { get; set; }
        public int CompanyId { get; set; }
    }

    public class StyleViewModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            StyleViewModel m = (StyleViewModel)base.BindModel(controllerContext, bindingContext);
            var request = controllerContext.HttpContext.Request;
            StringBuilder sb = new StringBuilder();
            String s;
            decimal subtotal = 0, total = 0;
            // Get CompanyID
            s = request.Form.Get("CompanyID");
            Int32.TryParse(s, out int coID);
            // JewelryTypeID
            sb.Clear();
            sb.AppendFormat("JewelryTypeID");
            s = request.Form.Get(sb.ToString());
            Int32.TryParse(s, out int jtid);
            m.Style.JewelryTypeId = jtid;
            // build Metals
            if (m.Metals != null)
            {
                for (int i = 0; i < m.Metals.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Metals[i].Id = id;
                    m.Metals[i].CompanyId = coID;

                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Metals[i].ComponentTypeId = ctid;

                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Metals[i].Name = s;
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Metals[i].Vendor = s;
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].VendorID", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Metals[i].VendorID = vId;
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Metal", i);
                    s = request.Form.Get(sb.ToString());
                    m.Metals[i].Metal = s;
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Price", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal price);
                    m.Metals[i].Price = price;
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Labor", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal labor);
                    m.Metals[i].Labor = labor;
                    sb.Clear();
                    sb.AppendFormat("Metals[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Metals[i].Qty = q;
                    m.Metals[i].Total = q * price + labor;
                    subtotal += m.Metals[i].Total;
                }
            }
            m.MetalsTotal = subtotal;
            total += m.MetalsTotal;
            // build Stones
            subtotal = 0;
            if (m.Stones != null)
            {
                for (int i = 0; i < m.Stones.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Stones[i].Id = id;
                    m.Stones[i].CompanyId = coID;

                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Stones[i].ComponentTypeId = ctid;

                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].Name = s;
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].Vendor = s;
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].VendorID", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Stones[i].VendorID = vId;
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].CtWt", i);
                    Int32.TryParse(s, out int ctwt);
                    m.Stones[i].CtWt = ctwt;
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Size", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].Size = s;
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].PPC", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal PPC);
                    m.Stones[i].PPC = PPC;
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Stones[i].Qty = q;
                    m.Stones[i].Total = q * PPC;
                    subtotal += m.Stones[i].Total;
                }
            }
            m.StonesTotal = subtotal;
            total += m.StonesTotal;
            // build Findings
            subtotal = 0;
            if (m.Findings != null)
            {
                for (int i = 0; i < m.Findings.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Findings[i].Id = id;
                    m.Findings[i].CompanyId = coID;

                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Findings[i].ComponentTypeId = ctid;

                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Name = s;
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Vendor = s;
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].VendorID", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Findings[i].VendorID = vId;
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Metal", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Metal = s;
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Price", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal price);
                    m.Findings[i].Price = price;
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Findings[i].Qty = q;
                    m.Findings[i].Total = q * price;
                    subtotal += m.Findings[i].Total;
                }
            }
            m.FindingsTotal = subtotal;
            total += m.FindingsTotal;

            // build Labors
            subtotal = 0;
            if (m.Labors != null)
            {
                for (int i = 0; i < m.Labors.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Labors[i].Id = id;
                    m.Labors[i].CompanyId = coID;

                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Labors[i].ComponentTypeId = ctid;

                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Labors[i].Name = s;
                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].PPH", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal pph);
                    m.Labors[i].PPH = pph;
                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].PPP", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal ppc);
                    m.Labors[i].PPP = ppc;
                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Labors[i].Qty = q;
                    m.Labors[i].Total = q * (ppc + pph);
                    subtotal += m.Labors[i].Total;
                }
            }
            m.LaborsTotal = subtotal;
            total += m.LaborsTotal;

            subtotal = 0;
            // build Miscs
            if (m.Miscs != null)
            {
                for (int i = 0; i < m.Miscs.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Miscs[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Miscs[i].Id = id;
                    m.Miscs[i].CompanyId = coID;

                    sb.Clear();
                    sb.AppendFormat("Miscs[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Miscs[i].ComponentTypeId = ctid;

                    sb.Clear();
                    sb.AppendFormat("Miscs[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Miscs[i].Name = s;
                    sb.Clear();
                    sb.AppendFormat("Miscs[{0}].PPP", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal ppc);
                    m.Miscs[i].PPP = ppc;
                    sb.Clear();
                    sb.AppendFormat("Miscs[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Miscs[i].Qty = q;
                    m.Labors[i].Total = q * ppc;
                    subtotal += m.Labors[i].Total;
                }
            }
            m.MiscsTotal = subtotal;
            total += m.MiscsTotal;
            m.Total = total;
            return m;
        }
    }
}

