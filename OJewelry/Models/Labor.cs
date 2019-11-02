namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Labor")]
    public partial class Labor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Labor()
        {
            StyleLabors = new HashSet<StyleLabor>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Vendor { get; set; }

        [StringLength(50)]
        public string Desc { get; set; }

        public decimal? PricePerHour { get; set; }

        public decimal? PricePerPiece { get; set; }

        public decimal? Qty { get; set; }

        //public bool Auto { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleLabor> StyleLabors { get; set; }
    }
}
