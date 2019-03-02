using TestCosmosSQL.Application.Configuration;
using TestCosmosSQL.Application.DataTransferObjects;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestCosmosSQL.Application.Services
{

    /// <summary>
    /// Sample Service Implementation
    /// </summary>
    public class TestCosmosSQLService : ITestCosmosSQLService
    {
        public readonly IOptionsMonitor<TestCosmosSQLConfiguration> _configuration;
        public TestCosmosSQLService(IOptionsMonitor<TestCosmosSQLConfiguration> configuration)
        {
            _configuration = configuration;
        }
        /// <inheritdoc />
        public Task<PlaceCreateDto> GetResourceAsync(Guid id, CancellationToken cancellationToken)
        {
            TestCosmosSQLConfiguration config = _configuration.CurrentValue;

            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(new PlaceCreateDto());

        }
    }
}