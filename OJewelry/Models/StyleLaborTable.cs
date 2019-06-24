namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StyleLaborTable")]
    public partial class StyleLaborTable
    {
        public int Id { get; set; }

        public int StyleId { get; set; }

        public int LaborTableId { get; set; }

        public virtual LaborItem LaborItem { get; set; }

        public virtual Style Style { get; set; }
    }
}
