namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Presenter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Presenter()
        {
            Memos = new HashSet<Memo>();
            Contacts = new HashSet<Contact>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        private string shortname;

        [StringLength(10)]
        public string ShortName
        {
            get
            {
                if (shortname == null || shortname.Trim() =="")
                {
                    if (Name == null) return "";
                    shortname = Name.PadRight(10).Substring(0, 3).ToUpper();
                    return Name.PadRight(10).Substring(0, 3).ToUpper();
                } else {
                    return shortname.Trim();
                }
            }
            set {
                if (value != null)
                {
                    shortname = value.PadRight(10).Substring(0, 3);
                }
            }
        }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Memo> Memos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
