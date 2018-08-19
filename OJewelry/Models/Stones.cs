﻿namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Stones")]
    public partial class Stone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stone()
        {
            StyleStone = new HashSet<StyleStone>();
            /*Vendor= new Vendor();
            Company= new Company();
            Shape= new Shape();*/
            Price = 0;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CompanyId { get; set; }
        public int? VendorId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Desc { get; set; }

        public int? CtWt { get; set; }

        [StringLength(50)]
        public string StoneSize { get; set; }

        public int? ShapeId { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public virtual Company Company { get; set; }

        public virtual Vendor Vendor { get; set; }

        public virtual Shape Shape { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StyleStone> StyleStone { get; set; }
    }
}