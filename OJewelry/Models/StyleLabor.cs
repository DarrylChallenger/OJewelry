namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StyleLabor")]
    public partial class StyleLabor
    {
        public int Id { get; set; }

        public int StyleId { get; set; }

        public int LaborId { get; set; }

        public virtual Labor Labor { get; set; }

        public virtual Style Style { get; set; }
    }
}
