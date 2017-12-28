//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Component
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Component()
        {
            this.StyleComponents = new HashSet<StyleComponent>();
        }
    
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ComponentTypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PricePerHour { get; set; }
        public Nullable<decimal> PricePerPiece { get; set; }
        public string MetalMetal { get; set; }
        public Nullable<decimal> MetalLabor { get; set; }
        public Nullable<int> StonesCtWt { get; set; }
        public string StoneSize { get; set; }
        public Nullable<decimal> StonePPC { get; set; }
        public string FindingsMetal { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ComponentType ComponentType { get; set; }
        public virtual Vendor Vendor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleComponent> StyleComponents { get; set; }
    }
}
