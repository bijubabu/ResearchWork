using System;
using System.Collections.Generic;
using System.Text;

namespace TestCosmosSQL.Application.DataTransferObjects
{
    public class PlaceGetDto
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Entity type
        /// </summary>
        public string Type { get; set; }

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
