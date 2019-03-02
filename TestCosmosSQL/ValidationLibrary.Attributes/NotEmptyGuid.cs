using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationLibrary.Attributes
{
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        /// <inheritdoc />
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Guid guidValue)
            {
                return guidValue == Guid.Empty ? new ValidationResult("GUID cannot be Empty") : ValidationResult.Success;
            }

            return new ValidationResult("Input is not GUID");
        }
    }
}
