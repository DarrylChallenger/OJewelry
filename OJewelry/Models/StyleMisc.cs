namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StyleMisc")]
    public partial class StyleMisc
    {
        public int Id { get; set; }

        public int StyleId { get; set; }

        public int MiscId { get; set; }

        public virtual Misc Misc { get; set; }

        public virtual Style Style { get; set; }
    }
}
