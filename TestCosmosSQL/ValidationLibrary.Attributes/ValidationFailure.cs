using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ValidationLibrary.Attributes
{
    public class ValidationFailure : Exception
    {
        /// <summary>
        /// Validation Exception
        /// </summary>
        /// <param name="validationResults">Validation Results</param>
        public ValidationFailure(IEnumerable<ValidationResult> validationResults)
        {
            Message = string.Join(";", validationResults.Select(d => d.ErrorMessage));
        }

        /// <summary>
        /// Validation Exception
        /// </summary>
        /// <param name="message">Validation Message</param>
        public ValidationFailure(string message)
            : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// Message
        /// </summary>
        public override string Message { get; }
    }
}
