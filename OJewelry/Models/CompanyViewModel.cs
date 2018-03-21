using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace OJewelry.Models
{
    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            company = new Company();
            clients = new List<Client>();
            clients.Add(new Client());
            clients.Add(new Client());
            clients.Add(new Client());
            clients.Add(new Client());
        }
        public Company company { get; set; }
        public List<Client> clients { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredIfNotRemoved : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<string> items = new List<string>();

            if (value == null || value.ToString().Trim() == "")
            {
                return new ValidationResult("Validation error");
            }
            // Everything OK.
            return ValidationResult.Success;
        }
    }
}