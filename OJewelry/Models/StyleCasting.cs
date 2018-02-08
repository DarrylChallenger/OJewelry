namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StyleCasting
    {
        public int Id { get; set; }

        public int? StyleId { get; set; }

        public int? CastingId { get; set; }

        public virtual Casting Casting { get; set; }

        public virtual Style Style { get; set; }
    }
}
