using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ValidationLibrary.Attributes
{
    /// <inheritdoc />
    public class RequiredIfNotAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object[] DesiredValues { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Performs a validation if the value does not match the desired value
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        /// <param name="desiredValue">Desired Value</param>
        /// <param name="errorMessage">Error Message</param>
        public RequiredIfNotAttribute(string propertyName, object desiredValue, string errorMessage)
        {
            PropertyName = propertyName;
            DesiredValues = new[] { desiredValue };
            ErrorMessage = errorMessage;
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs a validation if the value does not match the desired value
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        /// <param name="desiredValues">Desired Values</param>
        /// <param name="errorMessage">Error Message</param>
        public RequiredIfNotAttribute(string propertyName, object[] desiredValues, string errorMessage)
        {
            PropertyName = propertyName;
            DesiredValues = desiredValues;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var type = instance.GetType();
            var propertyValue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (DesiredValues.Contains(propertyValue)) return ValidationResult.Success;
            switch (value)
            {
                case null:
                    return new ValidationResult(ErrorMessage);
                case IList list:
                    if (list.Count == 0)
                        return new ValidationResult(ErrorMessage);
                    break;
                case Guid id:
                    if (id == Guid.Empty)
                        return new ValidationResult(ErrorMessage);
                    break;
                case string data:
                    if (string.IsNullOrEmpty(data))
                        return new ValidationResult(ErrorMessage);
                    break;
            }

            return ValidationResult.Success;
        }
    }
}
