using DomainFramework.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;

namespace DomainFramework.DataAccess
{
    public static class HeaderExtensions
    {
        public static void AddPaging(
            this IHeaderDictionary header,
            CollectionQueryParameters queryParameters,
            int totalCount, 
            int maxPages = 500)
        {
            var skip = queryParameters.Skip ?? 0;

            var pageSize = queryParameters.Top ?? maxPages;

            int currentPage = (skip / pageSize) + 1;

            int totalPages = totalCount > 0
                ? (int)Math.Ceiling(totalCount / (double)pageSize)
                : 0;

            var paginationHeader = new
            {
                currentPage,
                pageSize,
                totalCount,
                totalPages
            };

            header.Add("X-Pagination", JsonConvert.SerializeObject(paginationHeader));
        }
    }
}
