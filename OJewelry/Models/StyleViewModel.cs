using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OJewelry.Models
{
    
    public class CastingComponent 
    {
        private Casting casting { get; set; }
        public CastingComponent()
        {
            casting = new Casting();
            Init();
        } // Comp.Vendor = new Vendor(); }
        public CastingComponent(Casting c) { casting = c; Init(); }
        void Init()
        {
            SVMState = SVMStateEnum.Dirty;
        }
        public int Id { get { return casting.Id; } set { casting.Id = value; } }

        [Required]
        public String Name { get { return casting.Name; } set { casting.Name = value; } }

        [Display(Name = "Quantity")]
        [Required]
        public int Qty { get; set; }
        public SelectList VendorList { get; set; }
        public SelectList MetalCodes { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [DataType(DataType.Currency)]
        public decimal? Price {
            get {
                return casting.Price ?? 0;
            }
            set {
                casting.Price = value;
            }
        }
        
        [Display(Name ="Labor")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [DataType(DataType.Currency)]

        public decimal? Labor
        {
            get
            {
                return casting.Labor ?? 0;
            }
            set
            {
                casting.Labor = value;
            }
        }

        public int VendorId
        {
            get { return casting.VendorId ?? 0; }
            set { casting.VendorId = value;  }
        }
        public int MetalCodeId
        {
            get { return casting.MetalCodeID ?? 0; }
            set { casting.MetalCodeID = value; }
        }

        public SVMStateEnum SVMState { get; set; }

        /*
         * [Display(Name="Vendor")]
        public String VendorName { get; set; }
        */
        [Display(Name ="Metal")]
        public String MetalCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
    }
    public class StoneComponent : StyleViewComponentModel
    {
        public StoneComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public StoneComponent(Component c) { Comp = c; }
        // VENDOR CT WT SIZE    PPC/$
        public int VendorId { get { return Comp.VendorId ?? 0; } set { Comp.VendorId = value; } }
        public int? CtWt { get { return Comp.StonesCtWt ?? 0; } set { Comp.StonesCtWt = value; } }
        public String Size { get { return Comp.StoneSize ?? ""; } set { Comp.StoneSize = value; } }

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPC { get { return Comp.StonePPC ?? 0; } set { Comp.StonePPC = value; } }
        public string VendorName { get { return Comp.Vendor.Name; } set { Comp.Vendor.Name = value; } }

    }
    public class FindingsComponent : StyleViewComponentModel
    {
        public FindingsComponent() { Comp = new Component(); Init(); } // Comp.Vendor = new Vendor(); }
        public FindingsComponent(Component c) { Comp = c; }
        // VENDOR	METAL	PRICE 
        public int VendorId { get { return Comp.VendorId ?? 0; } set { Comp.VendorId = value; } }
        public string VendorName { get { return Comp.Vendor.Name; } set { Comp.Vendor.Name = value; } }
        public String Metal { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? Price { get { return Comp.Price ?? 0; } set { Comp.Price = value; } }

    }
    public class LaborComponent 
    {
        private Labor labor { get; set; }
        public SVMStateEnum SVMState { get; set; }
        public LaborComponent() { labor = new Labor(); Init(); } // Comp.Vendor = new Vendor(); }
        public LaborComponent(Labor l) { labor = l; Init(); }
        void Init()
        {
            SVMState = SVMStateEnum.Dirty;
        }
        // PRICE/HR	PRICE/PC

        [Display(Name = "$/Hour")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPH { get { return labor.PricePerHour ?? 0; } set { labor.PricePerHour = value; } }

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPP { get { return labor.PricePerPiece ?? 0; } set { labor.PricePerPiece = value; } }
        
        public int Id { get { return labor.Id; } set { labor.Id = value; } }

        [Required]
        public String Name { get { return labor.Name; } set { labor.Name = value; } }
        public string Desc { get { return labor.Desc; } set { labor.Desc = value; } }

        [Display(Name = "Quantity")]
        public int? Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

    }
    public class MiscComponent
    {
        private Misc misc { get; set; }
        public MiscComponent() { misc = new Misc(); Init(); }
        public SVMStateEnum SVMState { get; set; }
        void Init()
        {
            SVMState = SVMStateEnum.Dirty;
        }
        public MiscComponent(Misc m) { misc = m; Init(); }
        // PRICE/PC

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPP { get { return misc.PricePerPiece ?? 0; } set { misc.PricePerPiece = value; } }

        public int Id { get { return misc.Id; } set { misc.Id = value; } }

        [Required]
        public String Name { get { return misc.Name; } set { misc.Name = value; } }
        public string Desc { get { return misc.Desc; } set { misc.Desc = value; } }

        [Display(Name = "Quantity")]
        public int? Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
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
        public int CompanyId { get { return Comp.CompanyId ?? 0; } set { Comp.CompanyId = value; } }
        public int ComponentTypeId { get { return Comp.ComponentTypeId; } set { Comp.ComponentTypeId = value; } }
        public Component Comp { get; set; }
        public String Name { get { return Comp.Name; } set { Comp.Name = value; } }
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public SVMStateEnum SVMState { get; set; }
    }

    public enum SVMStateEnum { Clean, Dirty, Added, Deleted }

    public class StyleViewModel
    {
        public StyleViewModel() { SVMState = SVMStateEnum.Dirty; } // For now, update all records
        public Style Style { get; set; }
        public List<CastingComponent> Castings { get; set; }
        public List<StoneComponent> Stones { get; set; }
        public List<FindingsComponent> Findings { get; set; }
        public List<LaborComponent> Labors { get; set; }
        public List<MiscComponent> Miscs { get; set; }
        public decimal MetalsTotal { get; set; }
        public decimal StonesTotal { get; set; }
        public decimal FindingsTotal { get; set; }
        public decimal LaborsTotal { get; set; }
        public decimal MiscsTotal { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public SVMStateEnum SVMState { get; set; }
        public int CompanyId { get; set; }
    }

    public partial class Casting
    {
        public Casting(CastingComponent c)
        {
            Set(c);
        }

        public void Set(CastingComponent c)
        {
            //Id = c.Id;
            Name = c.Name;
            VendorId = c.VendorId;
            MetalCodeID = c.MetalCodeId;
            Price = c.Price;
            Labor = c.Labor;
            Qty = c.Qty;
        }
    }

    public partial class Component
    {
        public Component(StoneComponent c)
        {
            Set(c);
        }

        public Component(FindingsComponent c)
        {
            Set(c);
        }

        public void Set(StoneComponent c)
        {
            //Id = c.Id;
            //CompanyId = c.CompanyId;
            //ComponentTypeId = c.ComponentTypeId;
            //VendorId = c.VendorId;
            //StonesCtWt = c.CtWt;
            //StoneSize = c.Size;
            //StonePPC = c.PPC;
        }

        public void Set(FindingsComponent c)
        {
            //Id = c.Id;
            //CompanyId = c.CompanyId;
            //ComponentTypeId = c.ComponentTypeId;
            //VendorId = c.VendorId;
            //Price = c.Price;
        }
    }

    public partial class Labor
    {
        public Labor(LaborComponent c)
        {
            Set(c);
        }

        public void Set(LaborComponent c)
        {
            //Id = c.Id;
            Name = c.Name;
            Desc = c.Desc;
            PricePerHour = c.PPH;
            PricePerPiece = c.PPP;
            Qty = c.Qty;
        }
    }

    public partial class Misc
    {
        public Misc(MiscComponent c)
        {
            Set(c);
        }

        public void Set(MiscComponent c)
        {
            //8Id = c.Id;
            Name = c.Name;
            Desc = c.Desc;
            PricePerPiece = c.PPP;
            Qty = c.Qty;

        }
    }

    public class StyleViewModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            StyleViewModel m = (StyleViewModel)base.BindModel(controllerContext, bindingContext);
            StringBuilder sb = new StringBuilder();
            String s;
            decimal subtotal = 0, total = 0;
            // Get CompanyID
            s = request.Form.Get("CompanyID");
            Int32.TryParse(s, out int coID);

            // Collection Id
            sb.Clear();
            sb.AppendFormat("CollectionId");
            s = request.Form.Get(sb.ToString());
            Int32.TryParse(s, out int collid);
            m.Style.CollectionId = collid;
            
            // JewelryTypeID
            sb.Clear();
            sb.AppendFormat("JewelryTypeID");
            s = request.Form.Get(sb.ToString());
            Int32.TryParse(s, out int jtid);
            m.Style.JewelryTypeId = jtid;
            // Metal Weight Id
            sb.Clear();
            sb.AppendFormat("MetalWtUnitId");
            s = request.Form.Get(sb.ToString());
            Int32.TryParse(s, out int mutid);
            m.Style.MetalWtUnitId = mutid;

            // build Metals
            
            if (m.Castings == null)
            { m.Castings = new List<CastingComponent>(); }
            else
            {
                for (int i = 0; i < m.Castings.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Castings[i].Id = id;

                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Castings[i].Name = s;

                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].VendorId", i); // vendor id
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Castings[i].VendorId= vId;

                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].MetalCodeID", i); // metalCode id
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int mcid);
                    m.Castings[i].MetalCodeId = mcid;

                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].Price", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal price);
                    m.Castings[i].Price = price;

                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].Labor", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal labor);
                    m.Castings[i].Labor = labor;

                    sb.Clear();
                    sb.AppendFormat("Castings[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Castings[i].Qty = q;
                    m.Castings[i].Total = q * price + labor;
                    subtotal += m.Castings[i].Total;
                }
            }
            m.MetalsTotal = subtotal;
            total += m.MetalsTotal;
            
            // build Stones
            subtotal = 0;
            if (m.Stones == null)
            { m.Stones = new List<StoneComponent>(); }
            else
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
                    /*
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].Vendor = s;
                    */
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].VendorId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Stones[i].VendorId = vId;
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
            if (m.Findings == null)
            { m.Findings = new List<FindingsComponent>(); }
            else
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
                    /*
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Vendor = s;
                    */
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].VendorId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Findings[i].VendorId = vId;
                    /* Changed to dropdown
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Metal", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Metal = s;
                    */
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
            if (m.Labors == null)
            { m.Labors = new List<LaborComponent>(); }
            else
            {
                for (int i = 0; i < m.Labors.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Labors[i].Id = id;

                    sb.Clear();
                    sb.AppendFormat("Labors[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);

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
            if (m.Miscs == null)
            { m.Miscs = new List<MiscComponent>();  }
            else
            {
                for (int i = 0; i < m.Miscs.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("Miscs[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.Miscs[i].Id = id;

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

                    subtotal += m.Miscs[i].Total;
                }
            }
            m.MiscsTotal = subtotal;
            total += m.MiscsTotal;
            
            m.Total = total;
            return m;
        }
    }
}

