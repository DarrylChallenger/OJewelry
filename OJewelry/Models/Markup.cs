using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [Required(ErrorMessage ="Please enter a Title. ")]
        [Display(Name ="Title")]
        public string title { get; set; }

        [Display(Name = "Multiplier")]
        public double multiplier { get; set; }

        [Display(Name = "Markup")]
        public double ratio { get; set; }
    }
}