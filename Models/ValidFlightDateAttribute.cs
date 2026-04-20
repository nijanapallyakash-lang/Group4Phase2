using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group4Flight.Models
{
    // Notice the addition of IClientModelValidator
    public class ValidFlightDateAttribute : ValidationAttribute, IClientModelValidator
    {
        // 1. The Server-Side Validation Logic
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime dateValue)
            {
                DateTime today = DateTime.Today;
                DateTime maxDate = today.AddYears(3);

                if (dateValue.Date > today && dateValue.Date <= maxDate)
                {
                    return ValidationResult.Success;
                }
                
                return new ValidationResult(ErrorMessage ?? "Date must be in the future and cannot exceed 3 years.");
            }

            return new ValidationResult("Invalid date format.");
        }

        // 2. The Client-Side HTML Tag Generator
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // This tells the browser: "Yes, this field requires validation"
            MergeAttribute(context.Attributes, "data-val", "true");
            
            // This links directly to your jQuery adapter: $.validator.unobtrusive.adapters.addBool("validflightdate")
            MergeAttribute(context.Attributes, "data-val-validflightdate", ErrorMessage ?? "Date must be in the future and cannot exceed 3 years.");
        }

        // Helper method to safely add attributes to the HTML tag
        private static void MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (!attributes.ContainsKey(key))
            {
                attributes.Add(key, value);
            }
        }
    }
}