using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.DataAccess
{
    public static class CollectionQueryExtensions
    {
        public static CollectionQuery<T> QueryParameters<T>(this CollectionQuery<T> query, CollectionQueryParameters queryParameters)
        {
            if (queryParameters.Skip != null)
            {
                query.Parameters(p => p.Name("$skip").Value(queryParameters.Skip.Value));
            }

            if (queryParameters.Top != null)
            {
                query.Parameters(p => p.Name("$top").Value(queryParameters.Top.Value));
            }

            if (queryParameters.Filter != null)
            {
                var sqlFilterBuilder = new SqlFilterBuilder(query.DatabaseDriver);

                var sqlFilter = sqlFilterBuilder.Build(queryParameters.Filter);

                query.Parameters(p => p.Name("$filter").Value(sqlFilter));
            }

            return query;
        }
    }
}
