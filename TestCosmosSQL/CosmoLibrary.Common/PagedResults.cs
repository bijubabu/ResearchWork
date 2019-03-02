using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CosmoLibrary.Common
{
    public class PagedResults<T>
    {
        public PagedResults(List<T> results, int pageSize, string nextPageToken)
        {
            Results = results;
            NextPageToken = nextPageToken;
            PageSize = pageSize;
        }

        public PagedResults(List<T> results, int pageSize, int pages, string nextPageToken)
        {
            Results = results;
            PageSize = pageSize;
            Pages = pages;
            NextPageToken = nextPageToken;
        }

        public readonly int Pages;

        public int PageSize;

        public List<T> Results { get; }

        public string NextPageToken { get; }

        public static implicit operator List<T>(PagedResults<T> results)
        {
            return results.Results;
        }
    }

}
