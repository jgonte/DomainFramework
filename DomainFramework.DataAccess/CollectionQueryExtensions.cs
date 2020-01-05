using DataAccess;
using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.DataAccess
{
    public static class CollectionQueryExtensions
    {
        public static CollectionQuery<T> QueryParameters<T>(this CollectionQuery<T> query, CollectionQueryParameters queryParameters)
        {
            if (queryParameters == null)
            {
                return query;
            }

            if (queryParameters.Skip != null)
            {
                query.Parameters(p => p.Name("$skip").Value(queryParameters.Skip.Value));
            }

            if (queryParameters.Top != null)
            {
                query.Parameters(p => p.Name("$top").Value(queryParameters.Top.Value));
            }

            if (queryParameters.Select != null)
            {
                if (queryParameters.Select.Any())
                {
                    query.Parameters(p => p.Name("$select").Value(string.Join(", ", queryParameters.Select)));
                }
                else
                {
                    query.Parameters(p => p.Name("$select").Value("*"));
                }
            }

            if (queryParameters.Filter != null)
            {
                var sqlFilterBuilder = new SqlFilterBuilder(query.DatabaseDriver);

                var sqlFilter = sqlFilterBuilder.Build(queryParameters.Filter);

                query.Parameters(p => p.Name("$filter").Value(sqlFilter));
            }

            if (queryParameters.OrderBy != null)
            {
                var sqlOrderByBuilder = new SqlOrderByBuilder(query.DatabaseDriver);

                var sqlOrderBy = sqlOrderByBuilder.Build(queryParameters.OrderBy);

                query.Parameters(p => p.Name("$orderby").Value(sqlOrderBy));
            }

            return query;
        }
    }
}
