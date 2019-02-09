using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OJewelry.Models
{
    public enum CVCMState { Clean, Dirty, Added, Deleted, Unadded }

    [NotMapped]
    public class CompanyViewClientModel 
    {
        public CompanyViewClientModel() { }
        public CompanyViewClientModel(Client client)
        {
            Id = client.Id;
            Name = client.Name;
            Phone = client.Phone;
            Email = client.Email;
            State = CVCMState.Dirty;
            JobTitle = client.JobTitle;
            CompanyID = client.CompanyID;
        }
        public Client GetClient() {
            return new Client(this);
        }
        
        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "Name")]
        //[Required(ErrorMessage = "Name is required.")]
        [RequiredIfNotRemoved]
        public string Name { get; set; }

        [StringLength(10)]
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        public int? CompanyID { get; set; }
        

        public CVCMState State { get; set; }
    }

    public class CompanyViewModel
    {
        public CompanyViewModel()
        {
            company = new Company();
            clients = new List<CompanyViewClientModel>();
        }
        public Company company { get; set; }
        public List<CompanyViewClientModel> clients { get; set; }
        // Default location (Presenter[0])

    }

    public partial class Client
    {
        public Client() { }
        public Client(CompanyViewClientModel cvcm)
        {
            Id = cvcm.Id;
            Name = cvcm.Name;
            Phone = cvcm.Phone;
            Email = cvcm.Email;
            JobTitle = cvcm.JobTitle;
            CompanyID = cvcm.CompanyID;

        }

        public Client(CompanyViewClientModel cvcm, int CompanyId)
        {
            Id = cvcm.Id;
            Name = cvcm.Name;
            Phone = cvcm.Phone;
            Email = cvcm.Email;
            JobTitle = cvcm.JobTitle;
            CompanyID = CompanyId;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredIfNotRemoved : ValidationAttribute
    { //This logic needs to be refactored. Needs to be in separate module and implementaion needs to be able to handle all cases
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<string> items = new List<string>();

            if (value == null || value.ToString().Trim() == "")
            {
                //return new ValidationResult(validationContext.MemberName + " Validation error.");
            }
            // Everything OK.
            return ValidationResult.Success;
        }
    }
}