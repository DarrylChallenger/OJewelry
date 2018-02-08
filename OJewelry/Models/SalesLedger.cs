namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SalesLedger")]
    public partial class SalesLedger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? StyleId { get; set; }

        [StringLength(512)]
        public string StyleInfo { get; set; }

        public int? UnitsSold { get; set; }

        public DateTime? Date { get; set; }

        [StringLength(512)]
        public string Notes { get; set; }

        public int? BuyerId { get; set; }

        public virtual Buyer Buyer { get; set; }
    }
}
