using System;
using System.Collections.Generic;
using System.Text;
using CosmoLibrary.Common;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmoLibrary.SqlDriver
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentDbRepository<TEntity>(this IServiceCollection services,
            RepositorySettings settings) where TEntity : DocumentEntity
        {
            services.AddSingleton<IDocumentDbRepository<TEntity>>(x => new GenericRepository<TEntity>(settings));
            return services;
        }

        public static IServiceCollection AddDocumentDbRepository<TEntity>(this IServiceCollection services,
            string databaseName, string collectionName, string endpointUri, string authKey,
            Action<RepositorySettings> settingsAction = null) where TEntity : DocumentEntity
        {
            return services.AddDocumentDbRepository<TEntity>(databaseName, collectionName, new Uri(endpointUri), authKey,
                settingsAction);
        }

        public static IServiceCollection AddDocumentDbRepository<TEntity>(this IServiceCollection services,
            string databaseName, string collectionName, Uri endpointUri, string authKey, Action<RepositorySettings> settingsAction = null,
            string overriddenCollectionName = "") where TEntity : DocumentEntity
        {
            var settings = new RepositorySettings(databaseName, collectionName, endpointUri, authKey);
            settingsAction?.Invoke(settings);
            return services.AddDocumentDbRepository<TEntity>(settings);
        }

        //public static IServiceCollection AddDocumentDbRepository<TEntity>(this IServiceCollection services, IDocumentClient docClient,
        //    string databaseName) where TEntity : DocumentEntity
        //{
        //    services.AddSingleton<IDocumentDbRepository<TEntity>>(x => new GenericRepository<TEntity>(docClient, databaseName));
        //    return services;
        //}
    }
}
