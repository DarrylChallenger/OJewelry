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
}

