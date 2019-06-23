namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using static OJewelry.Classes.Validations;

    public partial class ClientViewModel
    {
        public int Id { get; set; }

        [RequiredIfNotRemoved]
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public int? CompanyId { get; set; }

        public string CompanyName { get; set; }
    }
}
