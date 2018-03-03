using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OJewelry.Models
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            company = new Company();
            clients = new List<Client>();
            clients.Add(new Client());
        }
        public Company company { get; set; }
        public List<Client> clients { get; set; }
    }
}