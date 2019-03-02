using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ValidationLibrary.Attributes
{
    /// <inheritdoc />
    public class EnsureMinimumElementsAttribute : ValidationAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        private readonly int _minElements;
        public EnsureMinimumElementsAttribute(int minElements)
        {
            _minElements = minElements;
        }

        /// <inheritdoc />
        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                return list.Count >= _minElements;
            }
            return false;
        }
    }
}
