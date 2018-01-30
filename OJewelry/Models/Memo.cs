namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Memo")]
    public partial class Memo
    {
        public int Id { get; set; }

        public int PresenterID { get; set; }

        public int? StyleID { get; set; }

        public DateTime? Date { get; set; }

        public int Quantity { get; set; }

        [StringLength(255)]
        public string Notes { get; set; }

        public virtual Presenter Presenter { get; set; }

        public virtual Style Style { get; set; }
    }
}
