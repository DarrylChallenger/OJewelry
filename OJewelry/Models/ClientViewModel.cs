using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class ClientViewModel
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
        public int? CompanyId { get; set; }
        public String CompanyName { get; set; }
    }
}