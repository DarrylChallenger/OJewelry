using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using static OJewelry.Classes.Validations;

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
            //company = new Company();
            clients = new List<CompanyViewClientModel>();
        }
        //private Company company { get; set; }
        /*
         * public Company GetCompany()
        {
            return company;
        }
        public void SetCompany(Company value)
        {
            company = value;
        }*/
        public int Id { get; set; }
        /*
        {
            get { return company.Id; }
            set { company.Id = value; }
        }*/

        [StringLength(50)]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "A Name is required.")]
        public string Name { get; set; }
        /*
        {
            get { return company.Name; }
            set { company.Name = value; }
        }
        */
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        [Required(ErrorMessage = "A Phone Number  is required.")]
        public string Phone { get; set; }
        /*
        {
            get { return company.Phone; }
            set { company.Phone = value; }
        }
        */
        [StringLength(50)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "An Email Address is required.")]
        public string Email { get; set; }
        /*
        {
            get { return company.Email; }
            set { company.Email = value; }
        }
        */
        [Display(Name = "Addr 1")]
        public string StreetAddr { get; set; }
        /*
        {
            get { return company.StreetAddr; }
            set { company.StreetAddr = value; }
        }*/
        [Display(Name = "Addr 2")]
        public string Addr2 { get; set; }
        /*
        {
            get { return company.Addr2; }
            set { company.Addr2 = value; }
        }
        */
        public string Website { get; set; }
        /*
        {
            get { return company.Website; }
            set { company.Website = value; }
        }
        */
        /*
        public ICollection<CompanyUser> CompanyUsers
        {
            get { return company.CompanyUsers; }
            set { company.CompanyUsers = value; }
        }
        public ICollection<Client> Clients
        {
            get { return company.Clients; }
            set { company.Clients = value; }
        }   
        */
        public int? defaultStoneVendor { get; set; }
        /*
        {
            get { return company.defaultStoneVendor; }
            set { company.defaultStoneVendor = value; }
        }
        */
        /*
        public 
        {
            get { return company.; }
            set { company. = value; }
        };
        */
        public List<CompanyViewClientModel> clients { get; set; }
        // Default location (Presenter[0])

    }

    public partial class Company
    {
        public Company(CompanyViewModel cvm)
        {
            Id = cvm.Id;
            Name = cvm.Name;
            Phone = cvm.Phone;
            Email = cvm.Email;
            StreetAddr = cvm.StreetAddr;
            Addr2 = cvm.Addr2;
            Website = cvm.Website;
            defaultStoneVendor = cvm.defaultStoneVendor;
        }

        public void Set(CompanyViewModel cvm)
        {
            Id = cvm.Id;
            Name = cvm.Name;
            Phone = cvm.Phone;
            Email = cvm.Email;
            StreetAddr = cvm.StreetAddr;
            Addr2 = cvm.Addr2;
            Website = cvm.Website;
            defaultStoneVendor = cvm.defaultStoneVendor;
        }
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

}