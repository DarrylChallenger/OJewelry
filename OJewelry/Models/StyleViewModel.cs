using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Facebook;
using OJewelry.Classes;
using static OJewelry.Classes.Validations;

namespace OJewelry.Models
{
    public class CastingComponent
    {
        private Casting _casting = new Casting();
        public CastingComponent()
        {
            Init();
        }

        public CastingComponent(CastingComponent cc, Vendor v)
        {
            Init();
            Name = cc.Name;
            Qty = cc.Qty;
            Price = cc.Price;
            Labor = cc.Labor;
            VendorId = cc.VendorId;
            MetalCodeId = cc.MetalCodeId;
            MetalWeight = cc.MetalWeight;
            MetalWtUnitId = cc.MetalWtUnitId;
            MetalCode = cc.MetalCode;
            Total = Price.GetValueOrDefault() * Qty;
            this._casting.Vendor = v;
        }

        public CastingComponent(Casting c)
        {
            _casting = c;
            Init();
        }
        void Init()
        {
            State = SVMStateEnum.Dirty;
        }
        public int Id { get { return _casting.Id; } set { _casting.Id = value; } }

        //[Required]
        [RequiredIfNotRemoved]
        public String Name { get { return _casting.Name; } set { _casting.Name = value; } }

        [Display(Name = "Quantity")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N0}")]
        [DataType(DataType.Currency)]
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Quantity must not be negative.")]
        public decimal Qty { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [DataType(DataType.Currency)]
        public decimal? Price {
            get {
                return _casting.Price ?? 0;
            }
            set {
                _casting.Price = value;
            }
        }

        [Display(Name = "Labor")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [DataType(DataType.Currency)]

        public decimal? Labor
        {
            get
            {
                return _casting.Labor ?? 0;
            }
            set
            {
                _casting.Labor = value;
            }
        }

        public int VendorId
        {
            get { return _casting.VendorId ?? 0; }
            set { _casting.VendorId = value; }
        }

        [Display(Name = "Metal")]
        public String MetalCode { get; set; }

        [Display(Name = "WEIGHT")]
        public decimal MetalWeight
        {
            get { return _casting.MetalWeight; }
            set { _casting.MetalWeight = value; }
        }

        public int MetalCodeId
        {
            get { return _casting.MetalCodeID ?? 0; }
            set { _casting.MetalCodeID = value; }
        }

        public SVMStateEnum State { get; set; }
      
        [Display(Name="Vendor")]
        public String VendorName {
            get {
                return _casting.Vendor.Name;
            }
        }
        
        [Display(Name ="UNIT")]
        public int MetalWtUnitId
        {
            get { return _casting.MetalWtUnitId; }
            set { _casting.MetalWtUnitId = value; }
        }

        public SelectList CastingsVendorList { get; set; }
        public SelectList MetalCodes { get; set; }
        public SelectList MetalWeightUnits { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        /*
        public void SetVendorsList(List<Vendor> vendors, int defaultVendorSelection)
        {
            VendorList = new SelectList(vendors, "Id", "Name", defaultVendorSelection);
        }
        */

        public void SetCastingsVendorsList(List<Vendor> vendors, int defaultVendorSelection)
        {
            CastingsVendorList = new SelectList(vendors, "Id", "Name", defaultVendorSelection);
        }

        public void SetMetalsList(List<MetalCode> metals, int defaultMetalSelection)
        {
            MetalCodes = new SelectList(metals, "Id", "Code", defaultMetalSelection);
        }

        public void SetMetalWeightUnitsList(List<MetalWeightUnit> units, int defaultMetailWeightUnit)
        {
            MetalWeightUnits = new SelectList(units, "Id", "Unit", defaultMetailWeightUnit);
        }

        public decimal ComputePrice(decimal market, float multiplier, string unitCode) // use formula based on metal and multipliers
        {
            // Material type market * multiplier * weight (1gr = .643015DWT, 1DWT = 1.55517gr)
            double unitMultiplier;

            //unitCode = "DWT";
            if (unitCode.Trim() == "DWT")
            {
                unitMultiplier = 1;
            }
            else
            {
                unitMultiplier = 1.555;//  .643015;
            }
            decimal price = (decimal)((double)multiplier * (double)MetalWeight * unitMultiplier);
            //price = ((price * 100) + (decimal).5) % 100;
            price = price * 100;
            price = price + (decimal).5;
            price = Math.Floor(price);
            price = price / 100;
            return price;
        }
    }
    public class StoneComponent 
    {
        private Stone _stone = new Stone();
        public SVMStateEnum State { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public SelectList StoneList { get; set; }
        public SelectList ShapeList { get; set; }
        public SelectList SizeList { get; set; }

        public StoneComponent()
        {
            _stone.Vendor = new Vendor();
            Init();
        } // Comp.Vendor = new Vendor(); }

        public StoneComponent(StoneComponent sc)
        {
            _stone.Vendor = new Vendor();
            Init();
            CompanyId = sc.CompanyId;
            VendorId = sc.VendorId;
            VendorName = sc.VendorName ?? "";
            Name = sc.Name;
            Desc = sc.Desc;
            CtWt = sc.CtWt;
            Size = sc.Size;
            Price = sc.Price;
            SettingCost = sc.SettingCost;
            ShapeId = sc.ShapeId;
            Id = sc.Id;
            ShId = sc.ShId;
            SzId = sc.SzId;
            Qty = sc.Qty;
            
            Total = Price * Qty;
        }

        public StoneComponent(Stone s)
        {
            _stone = s;
            if (_stone.Vendor != null)
            {
                _stone.Vendor = s.Vendor;
            } else
            {
                _stone.Vendor = new Vendor(); ;
            }
            // set link fields
            Init();
        }
        void Init()
        {
            State = SVMStateEnum.Dirty;
        }

        //[Required(ErrorMessage = "You must select a stone!")]
        public int? Id
        {
            get { return _stone.Id; }
            set { _stone.Id = value ?? 0; }
        }

        public int linkId { get; set; }

        public int? CompanyId
        {
            get { return _stone.CompanyId; }
            set { _stone.CompanyId = value; }
        }

        [Display(Name ="Vendor")]
        public int? VendorId
        {
            get { return _stone.VendorId; }
            set { _stone.VendorId = value; }
        }


        public String VendorName
        {
            get { return _stone.Vendor.Name; }
            set { _stone.Vendor.Name = value; }
        }

        /*[Required]
        [Display(Name = "Stone Name")]*/
        public String Name { get; set; }

        public String Desc
        {
            get { return _stone.Desc; }
            set { _stone.Desc = value; }
        }

        //[Required]
        public int? CtWt { get; set; }

        public String Size { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Price
        {
            get { return _stone.Price; }
            set { _stone.Price = value; }
        }

        [Display(Name = "Setting Cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal SettingCost
        {
            get { return _stone.SettingCost; }
            set { _stone.SettingCost = value; }
        }

        public virtual int ShapeId
        {
            get { return _stone.ShapeId ?? 0; }
            set { _stone.ShapeId = value; }
        }

        public virtual string ShId { get; set; }
        public virtual string SzId { get; set; }

        public virtual Company Company
        {
            get { return _stone.Company; }
        }

        public virtual Vendor Vendor
        {
            get { return _stone.Vendor; }
        }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must not be negative.")]
        public int Qty { get; set; }

        /*[Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPC
        {
            get { return _stone.StonePPC; }
            set { _stone.StonePPC = value; }
        }*/

        public void SetStonesList(List<StoneListItem> stones, string defaultSelection)
        {
            if (string.IsNullOrEmpty(defaultSelection))
            {
                StoneList = new SelectList(stones, "Id", "Name");
            }
            else
            {
                StoneList = new SelectList(stones, "Id", "Name", defaultSelection);
            }
        }

        public void SetShapesList(List<ShapeListItem> shapes, string defaultSelection)
        {
            if (string.IsNullOrEmpty(defaultSelection))
            {
                ShapeList = new SelectList(shapes, "Id", "Name");
            }
            else
            {
                ShapeList = new SelectList(shapes, "Id", "Name", defaultSelection);
            }
        }

        public void SetSizesList(List<StoneSizeListItem> sizes, string defaultSelection)
        {
            if (string.IsNullOrEmpty(defaultSelection))
            {
                SizeList = new SelectList(sizes, "Id", "Name");
            }
            else
            {
                SizeList = new SelectList(sizes, "Id", "Name", defaultSelection);
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            if (Id == 0)
            {
                results.Add(new ValidationResult("You must select a Stone"));
            }
            return results;
        }
    }
    public class FindingsComponent
    {
        private Finding _finding = new Finding();
        public SVMStateEnum State { get; set; }

        [Display(Name = "Quantity")]
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Quantity must not be negative.")]
        public decimal Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public SelectList CompList { get; set; }


        public FindingsComponent()
        {
            _finding.Vendor = new Vendor();
            Init();
        }

        public FindingsComponent(FindingsComponent fc)
        {
            _finding.Vendor = new Vendor();
            Init();
            Id = fc.Id;
            Qty = fc.Qty;
            CompanyId = fc.CompanyId;
            VendorId = fc.VendorId;
            Name = fc.Name;
            VendorName = fc.VendorName;
            Weight = fc.Weight;
            Price = fc.Price;
            Total = Price * Qty;
        }

        public FindingsComponent(Finding f)
        {
            _finding = f;
            // set link fields
            Init();
        }

        void Init()
        {
            State = SVMStateEnum.Dirty;
        }
        
        //[Required(ErrorMessage = "You must select a finding!")]
        public int? Id
        {
            get { return _finding.Id; }
            set { _finding.Id = value ?? 0; }
        }

        public int linkId { get; set; }

        public int? CompanyId
        {
            get { return _finding.CompanyId; }
            set { _finding.CompanyId = value; }
        }

        public int? VendorId
        {
            get { return _finding.VendorId; }
            set { _finding.VendorId = value; }
        }

        public string Name
        {
            get { return _finding.Name; }
            set { _finding.Name = value; }
        }
        public String VendorName
        {
            get { return _finding.Vendor.Name; }
            set { _finding.Vendor.Name = value; }
        }

        public decimal? Weight
        {
            get { return _finding.Weight; }
            set { _finding.Weight = value; }
        }
        [Display(Name = "Price")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Price
        {
            get { return _finding.Price; }
            set { _finding.Price = value; }
        }

        public void SetFindingsList(List<Finding> findings, int defaultSelection)
        {
            if (defaultSelection == -1)
            {
                CompList = new SelectList(findings, "Id", "Name");
            } else
            {
                CompList = new SelectList(findings, "Id", "Name", defaultSelection);
            }
        }
    }
    public class LaborComponent
    {
        private Labor _labor = new Labor();
        public LMState State { get; set; }
        public LaborComponent() { Init(); }

        public LaborComponent(LaborComponent lc, Vendor v)
        {
            Init();
            PPH = lc.PPH;
            PPP = lc.PPP;
            Name = lc.Name;
            VendorStr = lc.VendorStr;
            VendorId = lc.VendorId;
            Desc = lc.Desc;
            Qty = lc.Qty;
            Total = (PPP.GetValueOrDefault() + PPH.GetValueOrDefault()) * Qty.GetValueOrDefault();
            _labor.Vendor = v;
        }

        public LaborComponent(Labor l) { _labor = l; Init(); }
        void Init()
        {
            State = LMState.Dirty;
        }
        // PRICE/HR	PRICE/PC

        [Display(Name = "$/Hour")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPH { get { return _labor.PricePerHour ?? 0; } set { _labor.PricePerHour = value; } }


        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? PPP { get { return _labor.PricePerPiece ?? 0; } set { _labor.PricePerPiece = value; } }
        
        public int Id { get { return _labor.Id; } set { _labor.Id = value; } }

        //[Required]
        [RequiredIfNotRemoved]
        public String Name { get { return _labor.Name; } set { _labor.Name = value; } }
        public string Desc { get { return _labor.Desc; } set { _labor.Desc = value; } }

        [StringLength(50)]
        public String VendorStr { get { return _labor.VendorStr; } set { _labor.VendorStr = value; } }

        public int VendorId
        {
            get { return _labor.VendorId ?? 0; }
            set { _labor.VendorId = value; }
        }

        [Display(Name = "Vendor")]
        public String VendorName
        {
            get
            {
                return _labor.Vendor.Name;
            }
        }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must not be negative.")]
        public decimal? Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public SelectList LaborsVendorList { get; set; }

        public void SetLaborsVendorsList(List<Vendor> vendors, int defaultVendorSelection)
        {
            LaborsVendorList = new SelectList(vendors, "Id", "Name", defaultVendorSelection);
        }


    }

    public class LaborItemComponent
    {
        public LaborItem _laborItem = new LaborItem();
        public LMState State { get; set; }
        public LaborItemComponent() { Init(); _laborItem.Vendor = new Vendor(); }

        public LaborItemComponent(LaborItemComponent lic)
        {
            Init();
            _laborItem = lic._laborItem;
            laborItemId = lic.laborItemId;
            pph = lic.pph;
            ppp = lic.ppp;
            Name = lic.Name;
            Qty = lic.Qty;
            Total = (ppp.GetValueOrDefault() + ppp.GetValueOrDefault()) * Qty.GetValueOrDefault();
            VendorName = lic.VendorName;
        }
        public LaborItemComponent(LaborItem li) { _laborItem = li; Init(); }
        void Init()
        {
            State = LMState.Dirty;
        }
        // PRICE/HR	PRICE/PC

        public int linkId { get; set; }

        public int laborItemId { get; set; }

        [Display(Name = "$/Hour")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? pph { get { return _laborItem.pph; } set { _laborItem.pph = value; } }

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal? ppp { get { return _laborItem.ppp; } set { _laborItem.ppp = value; } }

        public int Id { get { return _laborItem.Id; } set { _laborItem.Id = value; } }

        //[Required]
        [RequiredIfNotRemoved]
        public String Name { get { return _laborItem.Name; } set { _laborItem.Name = value; } }

        public String VendorName { get { return _laborItem.Vendor.Name; } set { _laborItem.Vendor.Name = value; }  }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must not be negative.")]
        public decimal? Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
    }

    public class MiscComponent
    {
        private Misc _misc = new Misc();
        public MiscComponent() {  Init(); }
        public SVMStateEnum State { get; set; }
        void Init()
        {
            State = SVMStateEnum.Dirty;
        }

        public MiscComponent(MiscComponent mc)
        {
            Init();
            PPP = mc.PPP;
            Name = mc.Name;
            Desc = mc.Desc;
            Vendor = mc.Vendor;
            Qty = mc.Qty;
            Total = PPP * Qty.GetValueOrDefault();
        }

        public MiscComponent(Misc m) { _misc = m; Init(); }
        // PRICE/PC

        [Display(Name = "$/Piece")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PPP { get { return _misc.PricePerPiece ?? 0; } set { _misc.PricePerPiece = value; } }

        public int Id { get { return _misc.Id; } set { _misc.Id = value; } }

        [RequiredIfNotRemoved]
        public String Name { get { return _misc.Name; } set { _misc.Name = value; } }
        public string Vendor { get { return _misc.Vendor; } set { _misc.Vendor = value; } }
        public string Desc { get { return _misc.Desc; } set { _misc.Desc = value; } }

        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must not be negative.")]
        public int? Qty { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
    }

    public enum SVMStateEnum { Dirty, Added, Deleted, Unadded, Fixed }
    public enum LMState { Clean, Dirty, Added, Deleted, Unadded, Fixed }
    public enum SVMCCTypeEnum { Castings, Stones, Findings, Labors, LaborItems, Miscs }
    public enum SVMDelButtonPos { Left, Right }
    public enum SVMOperation { Create, Edit, Print }

    public class StoneListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ShapeListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class StoneSizeListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class StyleViewModel
    {
        public StyleViewModel()
        {
            SVMState = SVMStateEnum.Dirty; // For now, update all records
            DelBtnPos = SVMDelButtonPos.Right;
            SVMOp = SVMOperation.Edit;
            markups = new List<Markup>();
        }

        public StyleViewModel(StyleViewModel oldModel, OJewelryDB db)
        {
            SVMState = SVMStateEnum.Dirty; // For now, update all records
            DelBtnPos = SVMDelButtonPos.Right;
            SVMOp = SVMOperation.Edit;
            Style = new Style();
            Style.CreateNewFrom(oldModel.Style);
            Style.Collection = oldModel.Style.Collection;
            PostedImageFile = oldModel.PostedImageFile;
            bool bCreating = oldModel.SVMOp == SVMOperation.Create;

            // Lists
            //Castings = new List<CastingComponent>();
            Castings = oldModel.Castings.ConvertAll(x =>
            {
                Vendor v = db.Vendors.Find(x.VendorId);
                CastingComponent c = new CastingComponent(x, v)
                {

                    //State = SVMStateEnum.Added
                    State = (bCreating) ? SVMStateEnum.Added: x.State
                };
                return c;
            }).ToList();
            Stones = oldModel.Stones.ConvertAll((x =>
            {
                StoneComponent s = new StoneComponent(x)
                {
                    //State = SVMStateEnum.Added
                    State = (bCreating) ? SVMStateEnum.Added : x.State
                };
                return s;
            })).ToList();
            Findings = oldModel.Findings.ConvertAll((x =>
            {
                FindingsComponent f = new FindingsComponent(x)
                {
                    //State = SVMStateEnum.Added
                    State = (bCreating) ? SVMStateEnum.Added : x.State
                };
                return f;
            })).ToList();
            Labors = oldModel.Labors.ConvertAll((x =>
            {
                Vendor v = db.Vendors.Find(x.VendorId);
                LaborComponent l = new LaborComponent(x, v)
                {
                    //State = x.State == SVMStateEnum.Fixed ? SVMStateEnum.Fixed : SVMStateEnum.Added
                    State = x.State == LMState.Fixed ? LMState.Fixed : ((bCreating) ? LMState.Added : x.State)
                };
                return l;
            })).ToList();
            LaborItems = oldModel.LaborItems.ConvertAll((x =>
            {
                LaborItemComponent l = new LaborItemComponent(x)
                {
                    //State = x.State == SVMStateEnum.Fixed ? SVMStateEnum.Fixed : SVMStateEnum.Added
                    State = x.State == LMState.Fixed ? LMState.Fixed : ((bCreating) ? LMState.Added : x.State)
                };
                return l;
            })).ToList();
            Miscs = oldModel.Miscs.ConvertAll((x =>
            {
                MiscComponent m = new MiscComponent(x)
                {
                    //State = x.State == SVMStateEnum.Fixed ? SVMStateEnum.Fixed : SVMStateEnum.Added
                    State = x.State == SVMStateEnum.Fixed ? SVMStateEnum.Fixed : ((bCreating) ? SVMStateEnum.Added : x.State)
                };
                return m;
            })).ToList();
            //assemblyCost = new AssemblyCost();
            markups = oldModel.markups;
        }

        public Style Style { get; set; }
        public List<CastingComponent> Castings { get; set; }

        [GreaterThanZero]
        public List<StoneComponent> Stones { get; set; }

        [GreaterThanZero]
        public List<FindingsComponent> Findings { get; set; }

        public List<LaborComponent> Labors { get; set; }
        public List<LaborItemComponent> LaborItems { get; set;
        }
        public List<MiscComponent> Miscs { get; set; }
        public decimal MetalsTotal { get; set; }
        public decimal StonesTotal { get; set; }
        public decimal FindingsTotal { get; set; }
        public decimal LaborsTotal { get; set; }
        public decimal LaborItemsTotal { get; set; }
        public decimal MiscsTotal { get; set; }
        public string CopiedStyleName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public SVMStateEnum SVMState { get; set; }
        public SVMCCTypeEnum SVMCCType { get; set; }
        public SVMOperation SVMOp { get; set; }
        public int CompanyId { get; set; }
        public HttpPostedFileBase PostedImageFile { get; set; }

        public List<JewelryType> drpJewelryTypes { get; set; }
        public List<LaborItem> drpLaborItems { get; set; }

        //public List<Vendor> jsVendors { get; set; }
        public List<Vendor> jsCastingsVendors { get; set; }
        public List<Vendor> jsLaborsVendors { get; set; }
        public List<MetalCode> jsMetals { get; set; }
        public List<MetalWeightUnit> jsMetalWeightUnits { get; set; }
        public List<StoneListItem> jsStones { get; set; }
        public List<ShapeListItem> jsShapes { get; set; }
        public List<StoneSizeListItem> jsSizes { get; set; }
        public List<Finding> jsFindings { get; set; }

        public bool CCLastRow { get; set; }
        public bool CCHeaderRow { get; set; }
        public int CCRowSection { get; set; }
        public int CCRowIndex { get; set; }
        public int i { get; set; }
        public SVMDelButtonPos DelBtnPos { get; set; }
        public static string FinishingLaborName = "FINISHING LABOR";
        public static string SettingLaborName = "SETTING LABOR";
        public static string PackagingName = "PACKAGING";

        public List<Markup> markups;

        public void MarkDefaultEntriesAsFixed()
        {
            if (Labors != null && Labors.Count > 0)
            {
                if (Labors[0].Name == StyleViewModel.FinishingLaborName)
                {
                    Labors[0].State = LMState.Fixed;
                }
            }

            if (Miscs != null && Miscs.Count > 0)
            {
                if (Miscs[0].Name == StyleViewModel.PackagingName)
                {
                    Miscs[0].State = SVMStateEnum.Fixed;
                }
            }

        }
        
        public decimal AddStoneSettingCosts() 
        {
            decimal sum = 0;
            foreach (StoneComponent sc in Stones)
            {
                sum+=sc.SettingCost;
            }
            return sum;
        }

        public void PopulateDropDownData(OJewelryDB db)
        {
            drpJewelryTypes = db.JewelryTypes.Where(jt => jt.CompanyId == CompanyId).ToList();
            drpLaborItems = db.LaborTable.Where(li => li.CompanyId == CompanyId).ToList();

            //jsVendors = db.Vendors.Where(x => x.CompanyId == CompanyId).OrderBy(v => v.Name).ToList();
            jsCastingsVendors = db.Vendors.Where(x => x.CompanyId == CompanyId && (x.Type.Type & vendorTypeEnum.Casting) == vendorTypeEnum.Casting).OrderBy(v => v.Name).ToList();
            jsLaborsVendors = db.Vendors.Where(x => x.CompanyId == CompanyId && (x.Type.Type & vendorTypeEnum.Labor) == vendorTypeEnum.Labor).OrderBy(v => v.Name).ToList();
            jsMetals = db.MetalCodes.Where(x => x.CompanyId == CompanyId).OrderByDescending(m => m.Code).ToList();

            jsStones = db.Stones.Where(x => x.CompanyId == CompanyId)
            .Select((st) => new 
                {
                    Name = st.Name,
                }
            ).Distinct().ToList()
                .Select((x, index) => new StoneListItem
                {
                    Name = x.Name,
                    Id = x.Name
                }).ToList();

            jsShapes = db.Shapes.Where(x => x.CompanyId == CompanyId)
                .Select((sh) => new 
                    {
                        Name = sh.Name,
                    }
                ).ToList().Select((x, index) => new ShapeListItem
                    {
                        Name = x.Name,
                        Id = x.Name
                    }).ToList();

            jsSizes = db.Stones.Where(x => x.CompanyId == CompanyId)
                .Select((st) => new
                    {
                        Name = st.StoneSize,
                    }
                ).Distinct().ToList()
                .Select((x, index) => new StoneSizeListItem()
                {
                    Name = x.Name,
                    Id = x.Name
                }).ToList();

            jsFindings = db.Findings.Include("Vendor").Where(x => x.CompanyId == CompanyId).OrderBy(f => f.Name).ToList();
            jsMetalWeightUnits = db.MetalWeightUnits.OrderBy(mwu => mwu.Unit).ToList();
        }

        public void PopulateDropDowns(OJewelryDB db)
        {
            // reusables
            foreach (CastingComponent cstc in Castings)
            {
                //cstc.SetVendorsList(jsVendors, cstc.VendorId);
                cstc.SetCastingsVendorsList(jsCastingsVendors, cstc.VendorId);
                cstc.SetMetalsList(jsMetals, cstc.MetalCodeId);
                cstc.SetMetalWeightUnitsList(jsMetalWeightUnits, cstc.MetalWtUnitId);
            }
            foreach (StoneComponent stscm in Stones)
            {
                stscm.SetStonesList(jsStones, stscm.Name);//stscm.SetStonesList(jsStones, stscm.Id.Value);
                stscm.SetShapesList(jsShapes, stscm.ShId);//stscm.SetStonesList(jsStones, stscm.Id.Value);
                stscm.SetSizesList(jsSizes, stscm.SzId);//stscm.SetStonesList(jsStones, stscm.Id.Value);

            }
            foreach (FindingsComponent fiscm in Findings)
            {
                fiscm.SetFindingsList(jsFindings, fiscm.Id ?? 0); //fiscm.SetFindingsList(jsFindings, fiscm.Id.Value);
            }
            foreach (LaborComponent labc in Labors)
            {
                labc.SetLaborsVendorsList(jsLaborsVendors, labc.VendorId);

            }
        }

        public void PopulateComputedValues()
        {
            // castings price
            // labor stone setting costs
        }

        public void RepopulateComponents(OJewelryDB db)
        {
            decimal t = 0;
            List<StoneListItem> jsStonesWithDefault = jsStones.ToList();
            List<ShapeListItem> jsShapesWithDefault = jsShapes.ToList();
            List<StoneSizeListItem> jsSizesWithDefault = jsSizes.ToList();
            List<Finding> jsFindingsWithDefault = jsFindings.ToList();
            List<Stone> stoneSet = db.Stones.Where(st => st.CompanyId == this.CompanyId).Include(st => st.Shape).Include(st => st.Vendor).ToList();
            List<Finding> findingSet = db.Findings.Where(st => st.CompanyId == this.CompanyId).ToList();

            // Get Settings
            //GetSettingsCosts(db);
            drpJewelryTypes = db.JewelryTypes.Where(jt => jt.CompanyId == CompanyId).ToList();
            drpLaborItems = db.LaborTable.Where(li => li.CompanyId == CompanyId).ToList();

            int i = -1;
            foreach (StoneComponent sc in Stones)
            {
                i++;
                switch (sc.State)
                {
                    case SVMStateEnum.Added:
                        sc.VendorName = Stones[i].VendorName;
                        sc.CtWt = Stones[i].CtWt;
                        sc.Size = Stones[i].SzId;
                        sc.Qty = Stones[i].Qty;
                        sc.Price = Stones[i].Price;
                        //sc.Price = sc.ComputePrice(mc.Market, mc.Multiplier, db.MetalWeightUnits.Find(c.MetalWtUnitId)?.Unit);
                        sc.SettingCost = Stones[i].SettingCost;
                        sc.SetStonesList(jsStonesWithDefault, null);
                        sc.SetShapesList(jsShapesWithDefault, null);
                        sc.SetSizesList(jsSizesWithDefault, null);
                        sc.Total = Stones[i].Total;
                        sc.Desc = Stones[i].Desc;
                        break;
                    case SVMStateEnum.Unadded:
                        sc.VendorName = "";
                        sc.CtWt = 0;
                        sc.Size = "";
                        sc.Qty = 0;
                        sc.Price = 0;
                        sc.SettingCost = 0;
                        sc.SetStonesList(jsStonesWithDefault, null);
                        sc.SetShapesList(jsShapesWithDefault, null);
                        sc.SetSizesList(jsSizesWithDefault, null);
                        sc.Total = 0;
                        sc.Desc = "";

                        //sc.State = SVMStateEnum.Added;
                        break;
                    case SVMStateEnum.Dirty:
                    case SVMStateEnum.Fixed:
                    case SVMStateEnum.Deleted:
                        Stone c = db.Stones.Find(sc.Id);
                        //Stone q = stoneSet.FirstOrDefault(x => x.Id== sc.Id);
                        sc.VendorName = db.Vendors.Find(c.VendorId)?.Name;
                        //sc.CtWt = c.CtWt.Value;
                        sc.Size = c.StoneSize;
                        sc.Price = c.Price;
                        sc.SettingCost = c.SettingCost;
                        //sc.Qty = c.StyleStone.w;
                        sc.SetStonesList(jsStones, sc.Name);
                        sc.SetShapesList(jsShapes, sc.ShId);
                        sc.SetSizesList(jsSizes, sc.SzId); // seems wrong
                        t = sc.Price;
                        sc.Total = sc.Qty * t;
                        StonesTotal += sc.Total;
                        Total += sc.Total;
                        break;
                    default:
                        break;
                }
            }

            i = -1;
            foreach (LaborItemComponent lic in LaborItems)
            {
                i++;
                LaborItem li = db.LaborTable.Find(lic.laborItemId);

                switch (lic.State)
                {
                    case LMState.Added:
                        lic._laborItem = li;
                        lic.VendorName = li.Vendor.Name;
                        lic.pph = li.pph;
                        lic.ppp = li.ppp;
                        break;
                    case LMState.Unadded:
                        break;
                    case LMState.Fixed:
                    case LMState.Deleted:
                    case LMState.Dirty:
                        lic.VendorName = db.Vendors.Find(li.VendorId).Name;
                        break;
                    case LMState.Clean:
                    default:
                        break;
                }
            }
            i = -1;
            foreach (FindingsComponent fc in Findings)
            {
                i++;
                switch (fc.State)
                {
                    case SVMStateEnum.Added:
                        fc.VendorName = Findings[i].VendorName;
                        fc.Price = Findings[i].Price;
                        fc.Qty = Findings[i].Qty;
                        fc.Total = Findings[i].Total;
                        fc.SetFindingsList(jsFindingsWithDefault, -1);
                        break;
                    case SVMStateEnum.Unadded:
                        fc.VendorName = "";
                        fc.Price = 0;
                        fc.SetFindingsList(jsFindingsWithDefault, -1);
                        fc.Qty = 0;
                        fc.Total = 0;
                        break;
                    case SVMStateEnum.Dirty:
                    case SVMStateEnum.Fixed:
                    case SVMStateEnum.Deleted:
                        Finding c = db.Findings.Find(fc.Id);
                        fc.VendorName = db.Vendors.Find(c.VendorId)?.Name;
                        fc.Price = c.Price;
                        t = fc.Price;
                        fc.Total = fc.Qty * t;
                        FindingsTotal += fc.Total;
                        //fc.Qty = c.Qty ?? 0;
                        /* fc.SetFindingsList(jsFindings, fc.scId);*/
                        fc.SetFindingsList(jsFindings, fc.Id ?? 0);
                        Total += fc.Total;
                        break;
                    default:
                        break;
                }
            }
            // Add stone setting costs to total
            Total += AddStoneSettingCosts();
        }
        
        public void PopulateComponents(OJewelryDB db)
        {

            decimal t;
            // Stones
            foreach (StyleStone ss in Style.StyleStones)
            {
                Stone stone = db.Stones.Find(ss.StoneId);
                if (stone.VendorId == null)
                {

                }
                Shape shape = db.Shapes.Find(stone.ShapeId);
                StoneComponent stscm = new StoneComponent(stone);
                // Trouble when there is no Vendor defined!!!
                if (stone.VendorId == null)
                {
                    stscm.VendorName = "";
                }
                else
                {
                    stscm.VendorName = stone.Vendor.Name;
                }
                stscm.linkId = ss.Id;
                stscm.CtWt = stone.CtWt;
                stscm.Size = stone.StoneSize;
                stscm.Price = stone.Price;
                stscm.SettingCost = stone.SettingCost;
                stscm.Name = stone.Name;
                stscm.ShId = shape.Name;
                stscm.SzId = stone.StoneSize;
                stscm.SetStonesList(jsStones, stone.Name);
                stscm.SetShapesList(jsShapes, stscm.ShId);
                stscm.SetSizesList(jsSizes, stscm.SzId);
                stscm.Qty = ss.Qty ?? 0;
                t = stscm.Price;
                stscm.Total = stscm.Qty * t;
                StonesTotal += stscm.Total;
                Stones.Add(stscm);
                Total += stscm.Total;
            }

            // Findings
            foreach (StyleFinding sf in Style.StyleFindings)
            {
                Finding finding = db.Findings.Find(sf.FindingId);
                finding.Vendor = db.Vendors.Find(finding.VendorId) ?? new Vendor();
                FindingsComponent fiscm = new FindingsComponent(finding);
                fiscm.VendorName = finding.Vendor.Name;
                fiscm.linkId = sf.Id;
                fiscm.Price = finding.Price;
                t = fiscm.Price;
                fiscm.Qty = sf.Qty ?? 0;
                fiscm.Total = fiscm.Qty * t;
                FindingsTotal += fiscm.Total;
                fiscm.SetFindingsList(jsFindings, finding.Id);
                Findings.Add(fiscm);
                Total += fiscm.Total;
            }
        }

        public void LookupComponents(OJewelryDB db) // call only for copy & print
        {
            decimal t = 0, t2 = 0;
            Total = MetalsTotal = StonesTotal = FindingsTotal = LaborItemsTotal = LaborsTotal = MiscsTotal = 0;
            List<Casting> castingSet = db.Castings.ToList();
            List<Stone> stoneSet = db.Stones.Where(st => st.CompanyId == this.CompanyId).Include(st => st.Shape).Include(st => st.Vendor).ToList();
            List<Finding> findingSet = db.Findings.Where(st => st.CompanyId == this.CompanyId).ToList();
            JewelryType jt = db.JewelryTypes.Find(Style.JewelryTypeId);

            foreach (CastingComponent c in Castings.Where(x => x.State == SVMStateEnum.Added || x.State == SVMStateEnum.Dirty))
            {
                MetalCode mc = db.MetalCodes.Find(c.MetalCodeId);
                c.MetalCode = mc.Code;
                //c.MetalCode = ;
                t = c.ComputePrice(mc.Market, mc.Multiplier, db.MetalWeightUnits.Find(c.MetalWtUnitId)?.Unit);
                c.Price = t;
                t2 = c.Labor ?? 0;
                c.Total = c.Qty * (t + t2);
                MetalsTotal += c.Total;
            }
            Total += MetalsTotal;

            foreach (StoneComponent s in Stones)
            {
                Stone matchingStone = stoneSet.FirstOrDefault(sc => sc.Name == s.Name && sc.Shape.Name == s.ShId && sc.StoneSize == s.SzId);
                if (matchingStone != null)
                {
                    s.CtWt = matchingStone.CtWt;
                    s.VendorName = matchingStone.Vendor?.Name;
                    s.Price = matchingStone.Price;
                    s.Total = s.Price * s.Qty;
                    s.SettingCost = matchingStone.SettingCost;
                    StonesTotal += s.Total;
                }
            }
            Total += StonesTotal;

            foreach (FindingsComponent f in Findings)
            {
                Finding matchingFinding = findingSet.FirstOrDefault(fc => fc.Id == f.Id);
                if (matchingFinding != null)
                {
                    f.Name = matchingFinding.Name;
                    f.VendorName = matchingFinding.Vendor?.Name;
                    f.Weight = matchingFinding.Weight;
                    f.Price = matchingFinding.Price;
                    f.Total = f.Price * f.Qty;
                    FindingsTotal += f.Total;
                }
            }
            Total += FindingsTotal;

            if (Style.JewelryType.bUseLaborTable) {
                foreach (LaborItemComponent li in LaborItems)
                {
                    t = (li.ppp.GetValueOrDefault() + li.pph.GetValueOrDefault()) * li.Qty.Value;
                    li.Total = t;
                    LaborItemsTotal += li.Total;
                }
                Total += LaborItemsTotal;
            } else {
                foreach (LaborComponent l in Labors)
                {
                    if (l.Name == "FINISHING LABOR") // FINISHING LABOR (default entry)
                    {
                        l.PPP = jt.FinishingCost;
                    }
                    t = l.PPH.Value;
                    t2 = l.PPP.Value;
                    l.Total = l.Qty.Value * (t + t2);
                    LaborsTotal += l.Total;
                }
                Total += LaborsTotal;
            }

            foreach (MiscComponent m in Miscs)
            {
                if (m.Name == "PACKAGING") // PACKAGING (default entry)
                {
                    m.PPP = jt.PackagingCost;
                }
                t = m.PPP;
                m.Total = m.Qty.Value * t;
                MiscsTotal += m.Total;
            }
            Total += MiscsTotal;
            // Add stone setting costs to total
            Total += AddStoneSettingCosts();
        }

        public void Populate(int? id, OJewelryDB db)
        {
            decimal t = 0, t2 = 0;
            Castings = new List<CastingComponent>();
            Stones = new List<StoneComponent>();
            Findings = new List<FindingsComponent>();
            Labors = new List<LaborComponent>();
            LaborItems = new List<LaborItemComponent>();
            Miscs = new List<MiscComponent>();

            PopulateDropDownData(db);
            //GetSettingsCosts(db);

            if (id == null)
            {
                Style = new Style();
            }
            else
            {
                // Get Casting Links
                Style.StyleCastings = db.StyleCastings.Where(x => x.StyleId == Style.Id).ToList();
                // Get component links
                //Style.StyleComponents = db.StyleComponents.Where(x => x.StyleId == Style.Id).ToList();
                Style.StyleStones = db.StyleStones.Where(x => x.StyleId == Style.Id).ToList();
                Style.StyleFindings = db.StyleFindings.Where(x => x.StyleId == Style.Id).ToList();
                // Get Labor Links
                Style.StyleLabors = db.StyleLabors.Where(x => x.StyleId == Style.Id).ToList(); ;
                Style.StyleLaborItems = db.StyleLaborItems.Where(x => x.StyleId == Style.Id).ToList();
                // Get Misc Links
                Style.StyleMiscs = db.StyleMiscs.Where(x => x.StyleId == Style.Id).ToList(); ;
                // get components for each link
                // Metals
                foreach (StyleCasting sc in Style.StyleCastings)
                {
                    Casting casting = db.Castings.Find(sc.CastingId); // Castings
                    casting.Vendor = db.Vendors.Find(casting.VendorId);
                    CastingComponent cstc = new CastingComponent(casting);
                    // Need to get the vendor and metal code
                    cstc.VendorId = casting.VendorId.Value;
                    cstc.MetalCodeId = casting.MetalCodeID.Value;
                    cstc.MetalWtUnitId = casting.MetalWtUnitId;
                    cstc.MetalWeight = casting.MetalWeight;
                    //cstc.SetVendorsList(jsVendors, casting.VendorId.Value);
                    cstc.SetCastingsVendorsList(jsCastingsVendors, casting.VendorId.Value);
                    cstc.SetMetalsList(jsMetals, casting.MetalCodeID.Value);// 
                    //cstc.VendorName = db.Vendors.Find(casting.VendorId).Name; //  Vendor();
                    MetalCode mc = db.MetalCodes.Find(casting.MetalCodeID); 
                    cstc.MetalCode = mc.Code; // Metal Code
                    cstc.Qty = casting.Qty.Value;
                    t = cstc.ComputePrice(mc.Market, mc.Multiplier, db.MetalWeightUnits.Find(casting.MetalWtUnitId)?.Unit);
                    cstc.Price = t;
                    t2 = cstc.Labor ?? 0;
                    cstc.Total = cstc.Qty * (t + t2);
                    MetalsTotal += cstc.Total;
                    Castings.Add(cstc);
                    Total += cstc.Total;
                }

                PopulateComponents(db);
                // LaborItems
                foreach (StyleLaborTableItem sli in Style.StyleLaborItems)
                {
                    LaborItem li = db.LaborTable.Find(sli.LaborTableId);
                    LaborItemComponent liscm = new LaborItemComponent(li);
                    //liscm.Qty = sli.LaborItem.Qty ?? 0; Don't need here, no inventory for labor items
                    liscm.linkId = sli.Id;
                    liscm.laborItemId = sli.LaborTableId;
                    t = liscm.pph.GetValueOrDefault();
                    t2 = liscm.ppp.GetValueOrDefault();
                    liscm.Qty = sli.Qty;
                    liscm.Total = liscm.Qty.GetValueOrDefault() * (t + t2);
                    LaborItemsTotal += liscm.Total;
                    LaborItems.Add(liscm);
                    if (Style.JewelryType.bUseLaborTable) Total += liscm.Total;
                }
                // Labor
                foreach (StyleLabor sl in Style.StyleLabors)
                {
                    Labor lb = db.Labors.Find(sl.LaborId);
                    lb.Vendor = db.Vendors.Find(lb.VendorId);
                    LaborComponent liscm = new LaborComponent(lb);
                    liscm.Qty = sl.Labor.Qty ?? 0;
                    //liscm.PPP = Style.JewelryType.FinishingCost;
                    t = liscm.PPH ?? 0;
                    t2 = liscm.PPP ?? 0;
                    liscm.Total = liscm.Qty.Value * (t + t2);
                    LaborsTotal += liscm.Total;
                    Labors.Add(liscm);
                    if (!Style.JewelryType.bUseLaborTable) Total += liscm.Total;
                }
                // Misc
                foreach (StyleMisc sms in Style.StyleMiscs)
                {
                    Misc misc = db.Miscs.Find(sms.MiscId); 
                    MiscComponent miscm = new MiscComponent(misc);
                    miscm.Qty = sms.Misc.Qty ?? 0;
                    //miscm.PPP = Style.JewelryType.PackagingCost;
                    t = miscm.PPP;
                    miscm.Total = miscm.Qty.Value * t;
                    MiscsTotal += miscm.Total;
                    Miscs.Add(miscm);
                    Total += miscm.Total;
                }
                MarkDefaultEntriesAsFixed();
                // Add stone setting costs to total
                Total += AddStoneSettingCosts();

                PopulateDropDowns(db);
            }
        }
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
            MetalWeight = c.MetalWeight;
            MetalWtUnitId = c.MetalWtUnitId;
            Price = c.Price;
            Labor = c.Labor;
            Qty = c.Qty;
        }
    }

    public partial class Stone
    {
        public void Set(StoneComponent c)
        {
            Id = c.Id ?? 0;
            CompanyId = c.CompanyId;
            VendorId = c.VendorId;
            Name = c.VendorName;
            Price = c.Price;
            SettingCost = c.SettingCost;
        }

        public Stone(StoneComponent sc)
        {
            Set(sc);
        }
    }

    public partial class Finding
    {
        public void Set(FindingsComponent c)
        {
            Id = c.Id ?? 0;
            CompanyId = c.CompanyId;
            VendorId = c.VendorId;
            Price = c.Price;
        }

        public Finding(FindingsComponent fc)
        {
            Set(fc);
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
            VendorId = c.VendorId;
            VendorStr = c.VendorStr;
            Qty = c.Qty;
        }
    }

    public partial class LaborItem
    {
        public LaborItem(LaborItemComponent c)
        {
            Set(c);
        }

        public void Set(LaborItemComponent c)
        {
            //Id = c.Id;
            Name = c.Name;
            pph = c.pph;
            ppp = c.ppp;
            //Qty = c.Qty; there is no inventory of a labor
            //State = c.State;
            State = c.State;
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
            Vendor = c.Vendor;
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
            sb.AppendFormat("JewelryTypeId");
            s = request.Form.Get(sb.ToString());
            Int32.TryParse(s, out int jtid);
            //m.Style.JewelryTypeId = jtid; unneeded
            // Metal Weight Id
            sb.Clear();
            sb.AppendFormat("MetalWtUnitId");
            s = request.Form.Get(sb.ToString());
            Int32.TryParse(s, out int mutid);
            m.Style.MetalWtUnitId = mutid;

            // build Castings (aka Metals)
            
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
                    decimal.TryParse(s, out decimal q);
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

                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].linkId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int linkid);
                    m.Stones[i].linkId = linkid;
                    /*
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].Name = s;

                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].ShId", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].ShId = s;

                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].SzId", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].SzId = s;
                    */

                    /*
                    m.Stones[i].CompanyId = coID;

                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Stones[i].ComponentTypeId = ctid;

                    */
                    /*
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Stones[i].Vendor = s;
                    */
                    /*
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
                    */
                    sb.Clear();
                    sb.AppendFormat("Stones[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int q);
                    m.Stones[i].Qty = q;
                    //m.Stones[i].Total = q * PPC;
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

                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].linkId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int linkid);
                    m.Findings[i].linkId = linkid;

                    //m.Findings[i].CompanyId = coID;
                    /*
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].ComponentTypeId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int ctid);
                    m.Findings[i].ComponentTypeId = ctid;

                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Name = s;
                    */
                    /*
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Vendor", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Vendor = s;
                    */
                    /*
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].VendorId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int vId);
                    m.Findings[i].VendorId = vId;
                    */
                    /* Changed to dropdown
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Metal", i);
                    s = request.Form.Get(sb.ToString());
                    m.Findings[i].Metal = s;
                    */
                    /*
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Price", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal price);
                    m.Findings[i].Price = price;
                    */
                    sb.Clear();
                    sb.AppendFormat("Findings[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    decimal.TryParse(s, out decimal q);
                    m.Findings[i].Qty = q;
                    //m.Findings[i].Total = q * price;
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
                    sb.AppendFormat("Labors[{0}].Desc", i);
                    s = request.Form.Get(sb.ToString());
                    m.Labors[i].Desc = s;

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
                    Decimal.TryParse(s, out decimal q);
                    m.Labors[i].Qty = q;
                    m.Labors[i].Total = q * (ppc + pph);
                    subtotal += m.Labors[i].Total;
                }
            }
            m.LaborsTotal = subtotal;
            if (!m.Style.JewelryType.bUseLaborTable) total += m.LaborsTotal;

            // build LaborItems
            subtotal = 0;
            if (m.LaborItems == null)
            {
                m.LaborItems = new List<LaborItemComponent>();
            } else {
                for (int i = 0; i < m.LaborItems.Count; i++)
                {
                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].Id", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int id);
                    m.LaborItems[i].Id = id;

                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].linkId", i);
                    s = request.Form.Get(sb.ToString());
                    Int32.TryParse(s, out int linkid);
                    m.LaborItems[i].Id = linkid;

                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].Name", i);
                    s = request.Form.Get(sb.ToString());
                    m.LaborItems[i].Name = s;

                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].pph", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal pph);
                    m.LaborItems[i].pph = pph;

                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].ppp", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal ppc);
                    m.LaborItems[i].ppp = ppc;

                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].VendorName", i);
                    s = request.Form.Get(sb.ToString());
                    m.LaborItems[i].Name = s;

                    sb.Clear();
                    sb.AppendFormat("LaborItems[{0}].Qty", i);
                    s = request.Form.Get(sb.ToString());
                    Decimal.TryParse(s, out decimal q);
                    m.LaborItems[i].Qty = q;
                    m.LaborItems[i].Total = q * (ppc + pph);
                    subtotal += m.LaborItems[i].Total;
                }
            }
            m.LaborItemsTotal = subtotal;
            if (m.Style.JewelryType.bUseLaborTable) total += m.LaborsTotal; subtotal = 0;
            
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
                    sb.AppendFormat("Miscs[{0}].Desc", i);
                    s = request.Form.Get(sb.ToString());
                    m.Miscs[i].Desc = s;

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
                    subtotal += m.Miscs[i].Total*m.Miscs[i].Qty.Value;
                }
            }
            m.MiscsTotal = subtotal;
            total += m.MiscsTotal;
            
            m.Total = total;
            return m;
        }
    }

    public partial class Style
    {
        public void CreateNewFrom(Style oldStyle)
        {
            // clear out key values
            StyleNum = "";
            StyleName = oldStyle.StyleName;
            Desc = oldStyle.Desc;
            Id = 0;
            Quantity = 0;
            UnitsSold = 0;
            // members
            JewelryTypeId = oldStyle.JewelryTypeId;
            CollectionId = oldStyle.CollectionId;
            MetalWeight = oldStyle.MetalWeight;
            MetalWtUnitId = oldStyle.MetalWtUnitId;
            IntroDate = oldStyle.IntroDate;
            Width = oldStyle.Width;
            Length = oldStyle.Length;
            ChainLength = oldStyle.ChainLength;
            RetailPrice = oldStyle.RetailPrice;
            RetailRatio = oldStyle.RetailRatio;
            RedlineRatio = oldStyle.RedlineRatio;
            MetalWeightUnit = oldStyle.MetalWeightUnit;
            MetalWtNote = oldStyle.MetalWtNote;
            // classes
            JewelryType = oldStyle.JewelryType;
            MetalWeightUnit = oldStyle.MetalWeightUnit;
            Image = oldStyle.Image;
        }
    }
}


