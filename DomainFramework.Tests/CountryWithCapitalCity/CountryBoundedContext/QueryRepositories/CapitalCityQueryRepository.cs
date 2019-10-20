using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CapitalCityQueryRepository : EntityQueryRepository<CapitalCity, int?>
    {
        public override CapitalCity GetById(int? capitalCityId, IAuthenticatedUser user)
        {
            var result = Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetById]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(capitalCityId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<CapitalCity> GetByIdAsync(int? capitalCityId, IAuthenticatedUser user)
        {
            var result = await Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetById]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(capitalCityId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<CapitalCity> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Get]")
                .QueryParameters(queryParameters)
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<CapitalCity>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Get]")
                .QueryParameters(queryParameters)
                .ExecuteAsync();

            return result.Data;
        }

        public CapitalCity GetCapitalCityForCountry(string countryCode, IAuthenticatedUser user)
        {
            var result = Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetCapitalCity_ForCountry]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .Execute();

            return result.Data;
        }

        public async Task<CapitalCity> GetCapitalCityForCountryAsync(string countryCode, IAuthenticatedUser user)
        {
            var result = await Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetCapitalCity_ForCountry]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<CapitalCity>(new CapitalCityQueryRepository());
        }

    }
}