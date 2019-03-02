using TestCosmosSQL.Application.DataTransferObjects;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        Task<TestCosmosSQLDto> GetResourceAsync(Guid id, CancellationToken cancellationToken);
    }
}