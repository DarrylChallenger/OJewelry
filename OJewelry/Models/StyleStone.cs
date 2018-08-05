using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace OJewelry.Models
{

    [Table("StyleStone")]
    public partial class StyleStone
    {
        public int Id { get; set; }

        public int StyleId { get; set; }
        public int StoneId { get; set; }

        public int? Qty { get; set; }

        public virtual Style Style { get; set; }
        public virtual Stone Stone { get; set; }
        public virtual Company Company { get; set; }


    }
}
