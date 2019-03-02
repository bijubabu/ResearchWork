using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ValidationLibrary.Attributes;

namespace CosmoLibrary.Common
{
    public abstract  class DocumentEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [NotEmptyGuid]
        [JsonProperty(PropertyName="id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Entity type
        /// </summary>
        [Required]
        public string Type { get; set; }

        /// <summary>
        /// Validate model
        /// </summary>
        /// <returns></returns>
        public void Validate()
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                this,
                new ValidationContext(this, null, null),
                results,
                false);
            if(!isValid)
                throw new ValidationFailure(results);
        }

    }
}

