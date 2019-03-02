using System;
using System.ComponentModel.DataAnnotations;

namespace ValidationLibrary.Attributes
{
    /// <summary>
    /// Check a minimum value base on a flag
    /// </summary>
    public class EnsureMinBaseOnFlagAttribute : ValidationAttribute
    {
        /// <summary>
        /// Min value is flag is true
        /// </summary>
        private readonly int _minValueOnTrue;

        /// <summary>
        /// Name of the property to be check for flag
        /// </summary>
        private readonly string _flagProperty;

        /// <inheritdoc />
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="minValueOnTrue"></param>
        /// <param name="flagProperty"></param>
        public EnsureMinBaseOnFlagAttribute(int minValueOnTrue, string flagProperty)
        {
            _minValueOnTrue = minValueOnTrue;
            _flagProperty = flagProperty;
        }

        /// <inheritdoc />
        /// <summary>
        /// Check is Valid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var currentValue = (int) value;

            var flagProperty = validationContext.ObjectType.GetProperty(_flagProperty);

            if (flagProperty == null)
                throw new ArgumentException($"Property with this name {_flagProperty} not found");

            var flagValue = (bool) flagProperty.GetValue(validationContext.ObjectInstance);

            if (flagValue && currentValue < _minValueOnTrue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
