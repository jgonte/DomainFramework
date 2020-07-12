using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CapitalCityQueryRepository : EntityQueryRepository<CapitalCity, int>
    {
        public override (int, IEnumerable<CapitalCity>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<CapitalCity>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<CapitalCity> GetAll()
        {
            var result = Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<CapitalCity>> GetAllAsync()
        {
            var result = await Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override CapitalCity GetById(int capitalCityId)
        {
            var result = Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetById]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(capitalCityId)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<CapitalCity> GetByIdAsync(int capitalCityId)
        {
            var result = await Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetById]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(capitalCityId)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public CapitalCity GetCapitalCityForCountry(string countryCode)
        {
            var result = Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetCapitalCity]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .Execute();

            return result.Data;
        }

        public async Task<CapitalCity> GetCapitalCityForCountryAsync(string countryCode)
        {
            var result = await Query<CapitalCity>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetCapitalCity]")
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