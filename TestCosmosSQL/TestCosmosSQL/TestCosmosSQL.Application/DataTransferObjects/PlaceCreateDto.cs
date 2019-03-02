using System.Runtime.Serialization;

namespace TestCosmosSQL.Application.DataTransferObjects
{
    /// <summary>
    /// Represents a Sample Resource DTO
    /// </summary>

    public class PlaceCreateDto
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