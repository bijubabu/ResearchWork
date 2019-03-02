using System;
using System.Collections.Generic;
using System.Text;
using CosmoLibrary.Common;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace CosmoLibrary.SqlDriver
{
    internal class DocumentClientFactory
    {
        internal static IDocumentClient CreateDocumentClient(RepositorySettings settings)
        {
            return new DocumentClient(settings.EndpointUrl, settings.AuthKey,
                settings.ConnectionPolicy ?? ConnectionPolicy.Default);
        }

        internal static DocumentClient CreateDocumentClient(Uri endpoint, string authKeyOrResourceToken,
            ConnectionPolicy connectionPolicy = null)
        {
            return new DocumentClient(endpoint, authKeyOrResourceToken,
                connectionPolicy ?? ConnectionPolicy.Default);
        }

        internal static DocumentClient CreateDocumentClient(Uri endpoint, string authKeyOrResourceToken,
            JsonSerializerSettings jsonSerializerSettings, ConnectionPolicy connectionPolicy = null,
            ConsistencyLevel? desiredConsistencyLevel = null)
        {
            return new DocumentClient(endpoint, authKeyOrResourceToken, jsonSerializerSettings,
                connectionPolicy ?? ConnectionPolicy.Default, desiredConsistencyLevel);
        }
    }
}
