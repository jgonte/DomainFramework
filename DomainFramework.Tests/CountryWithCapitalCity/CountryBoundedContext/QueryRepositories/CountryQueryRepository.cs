using DataAccess;
using DomainFramework.Core;
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

        public override IEnumerable<Country> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Country>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Country>(new CountryQueryRepository());
        }

    }
}