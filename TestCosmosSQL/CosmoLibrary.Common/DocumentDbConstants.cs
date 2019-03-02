using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CosmoLibrary.Common
{
    public static class DocumentDbConstants
    {
        public const int MinimumCosmosThroughput = 400;

        public static readonly ConnectionPolicy DefaultConnectionPolicy =
            new ConnectionPolicy
            {
                ConnectionProtocol = Protocol.Https,
                ConnectionMode = ConnectionMode.Direct
            };

        public static readonly IndexingPolicy DefaultIndexingPolicy = 
            new IndexingPolicy(new RangeIndex(DataType.Number, -1), 
                new RangeIndex(DataType.String, -1), 
                new SpatialIndex(DataType.Point));
    }
}
