using System;
using System.Collections.Generic;
using System.Text;
using CosmoLibrary.Common;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CosmoLibrary.SqlDriver
{
    public class RepositorySettings
    {
        public string DatabaseName { get; }

        public string CollectionName { get; }

        internal string AuthKey { get; }

        public Uri EndpointUrl { get; }

        public ConnectionPolicy ConnectionPolicy { get; set; } = DocumentDbConstants.DefaultConnectionPolicy;

        public IndexingPolicy IndexingPolicy { get; set; } = DocumentDbConstants.DefaultIndexingPolicy;

        public int DefaultCollectionThroughput { get; set; } = DocumentDbConstants.MinimumCosmosThroughput;

        public string CollectionPrefix { get; set; } = string.Empty;

        public RepositorySettings(string databaseName, string collectionName, string endpointUrl, string authKey, Action<RepositorySettings> settings)
            : this(databaseName, collectionName, new Uri(endpointUrl), authKey, settings)
        {
        }

        public RepositorySettings(string databaseName, string collectionName, Uri endpointUrl, string authKey, Action<RepositorySettings> settings)
        {
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            EndpointUrl = endpointUrl ?? throw new ArgumentNullException(nameof(endpointUrl));
            AuthKey = authKey ?? throw new ArgumentNullException(nameof(authKey));
            settings?.Invoke(this);
        }

        public RepositorySettings(string databaseName, string collectionName, string endpointUrl, string authKey,
            ConnectionPolicy connectionPolicy = null, IndexingPolicy indexingPolicy = null, 
            int defaultCollectionThroughput = DocumentDbConstants.MinimumCosmosThroughput)
            : this(databaseName, collectionName, new Uri(endpointUrl), authKey,
                connectionPolicy, indexingPolicy, defaultCollectionThroughput)
        {
        }

        public RepositorySettings(string databaseName, string collectionName, Uri endpointUrl, string authKey, 
            ConnectionPolicy connectionPolicy = null, IndexingPolicy indexingPolicy = null, int defaultCollectionThroughput = DocumentDbConstants.MinimumCosmosThroughput)
        {
            DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
            EndpointUrl = endpointUrl ?? throw new ArgumentNullException(nameof(endpointUrl));
            AuthKey = authKey ?? throw new ArgumentNullException(nameof(authKey));

            ConnectionPolicy = connectionPolicy;
            DefaultCollectionThroughput = defaultCollectionThroughput;
            IndexingPolicy = indexingPolicy ?? DocumentDbConstants.DefaultIndexingPolicy;
        }
    }
}
