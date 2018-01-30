namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MetalCode
    {
        public int Id { get; set; }

        [StringLength(6)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Desc { get; set; }
    }
}
