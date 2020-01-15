using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OJewelry.Models
{

    [Table("StyleFinding")]
    public partial class StyleFinding
    {
        public int Id { get; set; }
        public int StyleId { get; set; }

        public int FindingId { get; set; }

        public decimal? Qty { get; set; }

        public virtual Finding Finding { get; set; }

        public virtual Style Style { get; set; }
    }
}
