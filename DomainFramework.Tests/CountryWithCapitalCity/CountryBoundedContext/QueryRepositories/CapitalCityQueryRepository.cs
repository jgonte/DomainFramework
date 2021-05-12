using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

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
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<CapitalCity>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<CapitalCity> GetAll()
        {
            var result = Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<CapitalCity>> GetAllAsync()
        {
            var result = await Query<CapitalCity>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetAll]")
                .ExecuteAsync();

            return result.Records;
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

            return result.Record;
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

            return result.Record;
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

            return result.Record;
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

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<CapitalCity>(new CapitalCityQueryRepository());
        }

    }
}