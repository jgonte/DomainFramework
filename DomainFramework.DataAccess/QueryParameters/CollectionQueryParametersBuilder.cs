using System;
using System.Linq;
using DomainFramework.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

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

                switch (query.Key)
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
                    case "$filter":
                        {
                            queryParameters.Filter = new ODataFilterBuilder()
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
