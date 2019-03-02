using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CosmoLibrary.Common;
using Microsoft.Azure.Documents.Linq;

namespace CosmoLibrary.SqlDriver
{
    /// <summary>
    /// Extension to To List Async
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// To List Async
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="queryable">queryable</param>
        /// <returns></returns>
        public static async Task<List<T>> ToListAsync<T>(this IDocumentQuery<T> queryable)
        {
            var list = new List<T>();
            while (queryable.HasMoreResults)
            {
                //Note that ExecuteNextAsync can return many records in each call
                var response = await queryable.ExecuteNextAsync<T>();
                list.AddRange(response);
            }

            return list;
        }

        /// <summary>
        /// To list Async
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="query">Query</param>
        /// <returns></returns>
        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> query)
        {
            return await query.AsDocumentQuery().ToListAsync();
        }


        /// <summary>
        /// To list Async
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="query">Query</param>
        /// <returns></returns>
        public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> query)
        {
            var list= await query.AsDocumentQuery().ToListAsync();
            return list.FirstOrDefault();
        }

        /// <summary>
        /// To paged list Async
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="query">Queryable</param>
        /// <returns>Task<CosmosPagedResults<T>></returns>
        public static async Task<PagedResults<T>> ToPagedListAsync<T>(this IQueryable<T> query)
        {
            return await query.AsDocumentQuery().ToPagedListAsync();
        }

        /// <summary>
        /// To Paged List Async
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="queryable">queryable</param>
        /// <returns>Task<CosmosPagedResults<T>></returns>
        public static async Task<PagedResults<T>> ToPagedListAsync<T>(this IDocumentQuery<T> queryable)
        {
            var list = new List<T>();
            var nextPageToken = string.Empty;

            var response = await queryable.ExecuteNextAsync<T>();
            list.AddRange(response);
            nextPageToken = response.ResponseContinuation;

            return new PagedResults<T>(list, 0, nextPageToken);
        }
    }
}

