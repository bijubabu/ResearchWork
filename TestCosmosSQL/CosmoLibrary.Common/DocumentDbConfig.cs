using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Documents;
using ValidationLibrary.Attributes;
using Microsoft.Azure.Documents.Client;

namespace CosmoLibrary.Common
{
    public class DocumentDbConfig
    {
        /// <summary>
        /// Cosmo Db Sql End Point
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Cosmo Offer Throughput
        /// </summary>
        public int CosmoOfferThroughput { get; set; } = DocumentDbConstants.MinimumCosmosThroughput;

        /// <summary>
        /// Database Name
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Collection Name
        /// </summary>
        public string CollectionName { get; set; }

        /// <summary>
        /// Cosmos Authentication Key
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// Cosmos Endpoint Url
        /// </summary>
        public Uri EndpointUrl { get; set; }

        /// <summary>
        /// Validate Configuration
        /// </summary>
        public void Validate()
        {
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(
                this,
                new ValidationContext(this, null, null),
                results,
                false);
            if (!isValid)
            {
                throw new ValidationFailure(results);
            }
        }
    }
}
