using System;
using System.Collections.Generic;
using System.Text;
using CosmoLibrary.Common;

namespace TestCosmosSQL.Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PlaceModel : DocumentEntity
    {
        /// <summary>
        /// Place Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Latitude of the place
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// Longitude of the place
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Place status
        /// </summary>
        public string Status { get; set; }
    }
}
