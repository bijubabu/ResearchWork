using TestCosmosSQL.Application.DataTransferObjects;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestCosmosSQL.Domain.Models;

namespace TestCosmosSQL.Application.Services
{
    /// <summary>
    /// Sample Service interface
    /// </summary>
    public interface ITestCosmosSQLService
    {
        /// <summary>
        /// Gets a resource
        /// </summary>
        /// <returns></returns>
        Task<PlaceCreateDto> GetResourceAsync(Guid id, CancellationToken cancellationToken);

        Task UpsertPlace(PlaceModel placeModel, CancellationToken cancellationToken);
    }
}