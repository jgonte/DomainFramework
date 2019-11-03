using DomainFramework.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace DomainFramework.DataAccess
{
    public class CollectionQueryParametersBuilder
    {
        private IQueryCollection _queryCollection;

        public CollectionQueryParametersBuilder(IQueryCollection queryCollection)
        {
            _queryCollection = queryCollection;
        }

        public CollectionQueryParameters Build()
        {
            var queryParameters = new CollectionQueryParameters();

            foreach (var query in _queryCollection)
            {
                var value = query.Value.SingleOrDefault();

                switch (query.Key.ToLowerInvariant())
                {
                    case "$top":
                        {
                            queryParameters.Top = int.Parse(value);
                        }
                        break;
                    case "$skip":
                        {
                            queryParameters.Skip = int.Parse(value);
                        }
                        break;
                    case "$select":
                        {
                            queryParameters.Select = value
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(f => f.Trim());
                        }
                        break;
                    case "$filter":
                        {
                            queryParameters.Filter = new ODataFilterBuilder()
                                .Build(value);
                        }
                        break;
                    case "$orderby":
                        {
                            queryParameters.OrderBy = new ODataOrderByBuilder()
                                .Build(value);
                        }
                        break;
                    default:
                        {
                            queryParameters.ExtraParameters.Add(query.Key, value);
                        }
                        break;
                }
            }

            return queryParameters;
        }
    }
}
