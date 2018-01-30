namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class StyleComponent
    {
        public int Id { get; set; }

        public int StyleId { get; set; }

        public int ComponentId { get; set; }

        public int? Quantity { get; set; }

        public virtual Component Component { get; set; }

        public virtual Style Style { get; set; }
    }
}
