using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CosmoLibrary.Common;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmoLibrary.SqlDriver
{
    public class GenericRepository<T> : IDocumentDbRepository<T> where T : DocumentEntity
    {
        private readonly DocumentDbConfig _cosmoConfiguration;

        /// <summary>
        /// The name of the database you want to connect to
        /// </summary>
        /// <remarks>This will be created if it doesn't exist</remarks>
        private readonly string _databaseName;

        /// <summary>
        /// The document collection you want to connect to. 
        /// </summary>
        /// <remarks>This will be created if it doesn't exist</remarks>
        private readonly string _collectionName;

        public string CollectionName { get; }

        public string DatabaseName { get; }

        /// <summary>
        /// Offer Throughput
        /// </summary>
        private readonly int _offerThroughput;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<GenericRepository<T>> _logger;

        /// <summary>
        /// The type for the items in the collection
        /// This is used to allow us to store multiple types of objects in the same collection
        /// </summary>
        public string ItemType => typeof(T).Name;

        /// <summary>
        /// The connection to the Azure DocumentDB. This should be injected and configured
        /// </summary>
        private readonly IDocumentClient _client;

        public RepositorySettings Settings { get; }

        public GenericRepository(RepositorySettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));

            _databaseName = settings.DatabaseName;
            _collectionName = settings.CollectionName;

            if (string.IsNullOrEmpty(Settings.DatabaseName))
                throw new ArgumentNullException(nameof(Settings.DatabaseName));

            _client = DocumentClientFactory.CreateDocumentClient(settings);

            InitializeRepository(_databaseName, _collectionName).GetAwaiter();
        }

        /// <summary>
        /// Constructor with IDocument client for unit testing
        /// </summary>
        /// <param name="client">Document Client</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="logger">Logger</param>
        public GenericRepository(IDocumentClient client, IOptions<DocumentDbConfig> configuration, ILogger<GenericRepository<T>> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            if (configuration == null || configuration.Value == null) throw new ArgumentNullException(nameof(configuration));
            if (configuration.Value == null) throw new ArgumentNullException(nameof(configuration));

            var cosmoConfiguration = configuration.Value;
            cosmoConfiguration.Validate();

            DatabaseName = cosmoConfiguration.DatabaseName;
            CollectionName = cosmoConfiguration.CollectionName;
            _offerThroughput = cosmoConfiguration.CosmoOfferThroughput;
            _logger = logger;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger">Logger</param>
        public GenericRepository(IOptions<DocumentDbConfig> configuration, ILogger<GenericRepository<T>> logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (configuration.Value == null) throw new ArgumentNullException(nameof(configuration));

            var cosmoConfiguration = configuration.Value;
            cosmoConfiguration.Validate();

            DocumentDbAccount.TryParse(cosmoConfiguration.ConnectionString, out _client);
            _databaseName = cosmoConfiguration.DatabaseName;
            _collectionName = cosmoConfiguration.CollectionName;
            _offerThroughput = cosmoConfiguration.CosmoOfferThroughput;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates database and collection if it doesn't exists
        /// </summary>
        /// <returns></returns>
        public async Task SetupAsync()
        {
            _logger.LogInformation("Setting Up Database and collection");
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName });
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName),
                new DocumentCollection { Id = _collectionName },
                new RequestOptions { OfferThroughput = _offerThroughput });
        }

        private async Task InitializeRepository(string databaseId, string collectionId)
        {
            var resName = (!string.IsNullOrEmpty(collectionId)) ? collectionId : typeof(T).Name;
            var collectionName = $"{Settings.CollectionPrefix ?? string.Empty}{resName}";

            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            await _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection { Id = collectionName },
                new RequestOptions { OfferThroughput = _offerThroughput });
        }

        /// <inheritdoc />
        /// <summary>
        /// Retrieve a specific document
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns>Document</returns>
        public async Task<T> GetAsync(Guid id)
        {
            try
            {
                var resp = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName,
                    id.ToString()));
                // ReSharper disable once SuspiciousTypeConversion.Global
                T item = (T)(dynamic)resp.Resource;
                return item;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                    throw new ResourceNotFoundException(id.ToString());
                throw;
            }
        }

        /// <summary>
        /// Get Document
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns>Document</returns>
        private async Task<Document> GetDocumentAsync(Guid id)
        {
            try
            {
                var resp = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, id.ToString()));
                return resp;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                    throw new ResourceNotFoundException(id.ToString());

                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Filter the result based on criteria set by expression
        /// </summary>
        /// <param name="predicate">Expression to use filter criteria</param>
        /// <returns>Collection of data</returns>
        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            var result = await _client
                .CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName))
                .Where(predicate).ToListAsync();
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Filter the result based on the expression criteria
        /// and produce the results in page
        /// </summary>
        /// <param name="predicate">Expression to use filter criteria</param>
        /// <param name="continuationToken">Continuation Token will know the next get</param>
        /// <param name="pageSize">Total size of the page</param>
        /// <returns>Collection of data along with last continuation token</returns>
        public async Task<PagedResults<T>> GetAsync(Expression<Func<T, bool>> predicate,
            string continuationToken, int pageSize)
        {
            var options = new FeedOptions
            {
                MaxItemCount = pageSize,
                EnableCrossPartitionQuery = true,
                RequestContinuation = string.IsNullOrEmpty(continuationToken) ? null : continuationToken
            };

            var result = await _client
                .CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), options)
                .Where(predicate ?? (d => true))
                .ToPagedListAsync();

            result.PageSize = pageSize;

            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Filter the result based on the expression criteria
        /// and produce the results in page
        /// </summary>
        /// <param name="predicate">Expression to use filter criteria</param>
        /// <param name="page">Next page to retrieve</param>
        /// <param name="pageSize">Total size of the page</param>
        /// <returns></returns>
        public Task<PagedResults<T>> GetAsync(Expression<Func<T, bool>> predicate, int page, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<T1> GetAsync<T1>(string sql, object parameters = null, FeedOptions feedOptions = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T1>> GetMultipleAsync<T1>(string sql, object parameters = null, FeedOptions feedOptions = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync(string id, RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<T> FindAsync(string id, object partitionKeyValue, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Get all the documents in a collection
        /// </summary>
        /// <returns>Collection of document</returns>
        public async Task<List<T>> GetAsync()
        {
            var result = await _client
                .CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName)).Where(d => d.Type == ItemType).ToListAsync();
            return result;
        }

        /// <inheritdoc />
        /// <summary>
        /// Update the document in already exists
        /// otherwise insert the document
        /// </summary>
        /// <param name="model">Document which need to be upsert</param>
        /// <returns></returns>
        public async Task UpsertAsync(T model, CancellationToken cancellationToken = default(CancellationToken))
        {
            model.Type = ItemType;
            model.Validate();

            await _client.UpsertDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName), model);
        }

        /// <inheritdoc />
        /// <summary>
        /// Delete a specific document
        /// </summary>
        /// <param name="id">Document id</param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var document = await GetDocumentAsync(id);
            if (document != null)
                await _client.DeleteDocumentAsync(document.SelfLink);
        }

        /// <inheritdoc />
        /// <summary>
        /// Check if the Document already exists
        /// </summary>
        /// <param name="id">Document Id</param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(Guid id)
        {
            try
            {
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _collectionName, id.ToString()));
                return true;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                    return false;
                throw;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Create a query for documents in a collection
        /// </summary>
        /// <returns>IQueryable{dynamic}</returns>
        public IQueryable<T> Query()
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
            var queryable = _client.CreateDocumentQuery<T>(collectionUri);
            return queryable;
        }

        //private void InitializeRepository(string overridenCollectionName)
        //{
        //    CollectionName = GetCosmosDbCollectionName(overridenCollectionName);

        //    _databaseCreator.EnsureCreatedAsync(DatabaseName).ConfigureAwait(false).GetAwaiter().GetResult();
        //    _collectionCreator.EnsureCreatedAsync<TEntity>(DatabaseName, CollectionName, Settings.DefaultCollectionThroughput, Settings.IndexingPolicy)
        //        .ConfigureAwait(false).GetAwaiter().GetResult();
        //}

        //private string GetCosmosDbCollectionName(string overridenCollectionName)
        //{
        //    var hasOverridenName = !string.IsNullOrEmpty(overridenCollectionName);
        //    return IsShared
        //        ? $"{Settings.CollectionPrefix ?? string.Empty}{(hasOverridenName ? overridenCollectionName : typeof(TEntity).GetSharedCollectionName())}"
        //        : $"{Settings.CollectionPrefix ?? string.Empty}{(hasOverridenName ? overridenCollectionName : typeof(TEntity).GetCollectionName())}";
        //}
    }
}

