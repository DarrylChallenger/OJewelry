using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OJewelry.Models;

namespace OJewelry.Classes
{
    public class Validations
    {
        [AttributeUsage(AttributeTargets.Property)]
        public sealed class RequiredIfNotRemoved : ValidationAttribute
        { //This logic needs to be refactored. Needs to be in separate module and implementaion needs to be able to handle all cases
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                List<string> items = new List<string>();
                var property = validationContext.ObjectType.GetProperty("State");
                var otherValue = property.GetValue(validationContext.ObjectInstance, null);
                if ((value == null || value.ToString().Trim() == "") && 
                    (otherValue.ToString() != "Deleted" && otherValue.ToString() != "Unadded") && otherValue.ToString() != "Fixed")
                {
                    return new ValidationResult(validationContext.DisplayName + " cannot be blank.");
                }
                // Everything OK.
                return ValidationResult.Success;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class HourlyXORStatic : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LaborItem item = (LaborItem)validationContext.ObjectInstance;
            var property = validationContext.ObjectType.GetProperty("State");
            if (property != null)
            {
                var state = property.GetValue(validationContext.ObjectInstance, null);
                if (state.ToString() == "Unadded" || state.ToString() == "Deleted" || state.ToString() == "Fixed")
                {
                    return ValidationResult.Success;
                }
            }

            if (((item.pph != null && item.ppp != null) && (item.pph != 0 && item.ppp != 0)) || (item.pph == null && item.ppp == null))
            {
                return new ValidationResult("Please specify either $/hour or $/piece, but not both");
            }
            return ValidationResult.Success;
        }
    }

}

