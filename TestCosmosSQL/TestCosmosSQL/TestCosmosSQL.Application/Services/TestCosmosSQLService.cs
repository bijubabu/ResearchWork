using TestCosmosSQL.Application.Configuration;
using TestCosmosSQL.Application.DataTransferObjects;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using CosmoLibrary.Common;
using TestCosmosSQL.Domain.Models;

namespace TestCosmosSQL.Application.Services
{

    /// <summary>
    /// Sample Service Implementation
    /// </summary>
    public class TestCosmosSQLService : ITestCosmosSQLService
    {
        public readonly IOptionsMonitor<TestCosmosSQLConfiguration> _configuration;
        /// <summary>
        /// Interface to communicate with repository
        /// </summary>
        private readonly IDocumentDbRepository<PlaceModel> _documentRepository;

        public TestCosmosSQLService(IOptionsMonitor<TestCosmosSQLConfiguration> configuration,
            IDocumentDbRepository<PlaceModel> documentDbRepository)
        {
            _configuration = configuration;
            _documentRepository = documentDbRepository;
        }

        /// <inheritdoc />
        public Task<PlaceCreateDto> GetResourceAsync(Guid id, CancellationToken cancellationToken)
        {
            TestCosmosSQLConfiguration config = _configuration.CurrentValue;

            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(new PlaceCreateDto());
        }

        public async Task UpsertPlace(PlaceModel placeModel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Update or insert the document to repository
            await _documentRepository.UpsertAsync(placeModel, cancellationToken);
        }
    }
}