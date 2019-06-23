namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using static OJewelry.Classes.Validations;

    public enum PVCMState { Clean, Dirty, Added, Deleted, Unadded }

    [NotMapped]
    public class PresenterViewContactModel
    {
        public PresenterViewContactModel() { }
        public PresenterViewContactModel(Contact contact)
        {
            Id = contact.Id;
            Name = contact.Name;
            Phone = contact.Phone;
            Email = contact.Email;
            JobTitle = contact.JobTitle;
            State = PVCMState.Dirty;
            PresenterId = contact.PresenterId;
        }

        public Contact GetContact()
        {
            return new Contact(this);
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

        public int? PresenterId { get; set; }


        public PVCMState State { get; set; }
    }
    public class PresenterViewModel
    {

        public PresenterViewModel()
        {
            Location = new Presenter();
            contacts = new List<PresenterViewContactModel>();
        }
        public string Phone { get; set; }
        public Presenter Location { get; set; }
        public List<PresenterViewContactModel> contacts { get; set; }

    }

    public partial class Contact
    {
        public Contact() { }
        public Contact(PresenterViewContactModel pvcm)
        {
            Id = pvcm.Id;
            Name = pvcm.Name;
            Phone = pvcm.Phone;
            Email = pvcm.Email;
            JobTitle = pvcm.JobTitle;
            PresenterId = pvcm.PresenterId;

            //CompanyId = pvcm.CompanyID;
        }

    }
}
