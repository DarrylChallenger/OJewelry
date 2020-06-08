using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static OJewelry.Classes.Validations;

namespace OJewelry.Models
{
    public class Markup
    {
        public Markup()
        {
            multiplier = 1;
            ratio = 1;
            State = MMState.Dirty;
        }
        public MMState State { get; set; }

        [RequiredIfNotRemoved(ErrorMessage ="Please enter a Title. ")]
        [Display(Name ="Title")]
        public string title { get; set; }

        [Display(Name = "Multiplier (%)")]
        public double multiplier { get; set; }

        [Display(Name = "Markup (%)")]
        public double ratio { get; set; }

        [Display(Name = "Margin (%)")]
        public double margin { get; set; }

        [DisplayFormat(ApplyFormatInEditMode =true, DataFormatString = "{0:N2}")]
        [Display(Name ="Addend ($)")]
        public double Addend { get; set; }
    }
}