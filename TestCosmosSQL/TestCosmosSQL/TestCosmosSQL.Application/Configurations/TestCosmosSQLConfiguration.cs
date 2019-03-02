using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CAP.ValidationLibrary.Attributes;

namespace TestCosmosSQL.Application.Configuration
{

    /// <summary>
    /// Sample Configuration. To use this file add the properties need for configuration and add those to the
    /// appsettings.json files for each environment
    /// </summary>
    public class TestCosmosSQLConfiguration
    {
        //Add Custom configuration properties here..

        /// <summary>
        /// Pod Namespace, this is set by an environment variable set in the K8 manifest file
        /// </summary>
        public string PodNamespace => Environment.GetEnvironmentVariable("POD_NAMESPACE");


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
            if (isValid == false)
            {
                throw new ValidationFailure(results);
            }
        }
    }

}
