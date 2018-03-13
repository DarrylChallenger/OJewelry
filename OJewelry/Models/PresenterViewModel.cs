namespace OJewelry.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class PresenterViewModel
    {
        public PresenterViewModel()
        {
            Location = new Presenter();
            contacts = new List<Contact>();
            contacts.Add(new Contact());
        }
        public Presenter Location { get; set; }
        public List<Contact> contacts { get; set; }
    }
}
