using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryQueryRepository : EntityQueryRepository<Country, string>
    {
        public override Country GetById(string countryCode, IAuthenticatedUser user)
        {
            var result = Query<Country>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetById]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Country> GetByIdAsync(string countryCode, IAuthenticatedUser user)
        {
            var result = await Query<Country>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetById]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override (int, IEnumerable<Country>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Country>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Country>(new CountryQueryRepository());
        }

    }
}