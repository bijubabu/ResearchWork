using System.ComponentModel.DataAnnotations;

namespace ValidationLibrary.Attributes
{
    /// <inheritdoc />
    /// <summary>
    /// Ensure Min Value
    /// </summary>
    public class EnsureMinimumValueAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private readonly int _minimumValue;
        public EnsureMinimumValueAttribute(int minimumValue)
        {
            _minimumValue = minimumValue;
        }

        /// <inheritdoc />
        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value is int valueToCompare)
            {
                return valueToCompare >= _minimumValue;
            }
            return false;
        }
    }
}
