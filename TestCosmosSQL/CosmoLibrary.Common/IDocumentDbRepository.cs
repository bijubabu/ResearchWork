using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;

namespace CosmoLibrary.Common
{
    /// <summary>
    /// Document Database Repository
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IDocumentDbRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Setup Database
        /// </summary>
        /// <returns></returns>
        Task SetupAsync();

        /// <summary>
        /// Dynamically search the database by passing an expression
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync();

        /// <summary>
        /// Retrieve a single object by ID
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Guid id);

        /// <summary>
        /// Dynamically search the database by passing an expression
        /// </summary>
        /// <param name="predicate">Search Expression</param>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Dynamically search the database by passing an expression based and limit number of result based page size and continuation token 
        /// </summary>
        /// <param name="predicate">Search Expression</param>
        /// <param name="continuationToken">Continuation Token</param>
        /// <param name="pageSize">Limit search result to page size</param>
        /// <returns>Task<CosmosPagedResults<T>></returns>
        Task<PagedResults<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, string continuationToken, int pageSize);

        /// <summary>
        /// Dynamically search the database by passing an expression based and limit number of result based page size and continuation token 
        /// </summary>
        /// <param name="predicate">Expression to filter the result</param>
        /// <param name="page">Page to be used</param>
        /// <param name="pageSize">Size of each page</param>
        /// <returns></returns>
        Task<PagedResults<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate, int page, int pageSize = 10);

        /// <summary>
        ///     Returns a single item of any type that matches the expression provided.
        /// </summary>
        /// <param name="sql">The sql query for this operation.</param>
        /// <param name="parameters">The sql parameters to replace if any</param>
        /// <param name="feedOptions">The feed options for this operation.</param>
        /// <param name="cancellationToken">The CancellationToken for this operation.</param>
        Task<T> GetAsync<T>(string sql, object parameters = null, FeedOptions feedOptions = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns a collection of items of any type that match the expression provided.
        /// </summary>
        /// <param name="sql">The sql query for this operation.</param>
        /// <param name="parameters">The sql parameters to replace if any</param>
        /// <param name="feedOptions">The feed options for this operation.</param>
        /// <param name="cancellationToken">The CancellationToken for this operation.</param>
        Task<IEnumerable<T>> GetMultipleAsync<T>(string sql, object parameters = null, FeedOptions feedOptions = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Returns an entity by document/entity id from the cosmos db store. If the collection is partitioned you will need to provide the
        ///     partition key value in the <see cref="RequestOptions"/>.
        /// </summary>
        /// <param name="id">The id of the document/entity.</param>
        /// <param name="requestOptions">The request options for this operation.</param>
        /// <param name="cancellationToken">The CancellationToken for this operation.</param>
        /// <returns>The entity that matches the id and partition key. Returns null if the entity is not found.</returns>
        Task<TEntity> FindAsync(string id, RequestOptions requestOptions = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        ///     Returns an entity by document/entity id and partition key value from the cosmos db store.
        /// </summary>
        /// <param name="id">The id of the document/entity.</param>
        /// <param name="partitionKeyValue">The partition key value.</param>
        /// <param name="cancellationToken">The CancellationToken for this operation.</param>
        /// <returns>The entity that matches the id and partition key. Returns null if the entity is not found.</returns>
        Task<TEntity> FindAsync(string id, object partitionKeyValue,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Insert or Update an object in the repository
        /// </summary>
        /// <param name="model">Model to insert or update</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpsertAsync(TEntity model, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Remove the object from the repository
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Exist by Id
        /// </summary>
        /// <param name="id">Model Id</param>
        /// <returns></returns>
        Task<bool> ExistAsync(Guid id);

        /// <summary>
        /// Query
        /// </summary>
        /// <typeparam name="TEntity">Model Id</typeparam>
        /// <returns></returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// The name of the database that this CosmosStore is targeting
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// The name of the collection that this CosmosStore is targeting
        /// </summary>
        string CollectionName { get; }

    }
}
