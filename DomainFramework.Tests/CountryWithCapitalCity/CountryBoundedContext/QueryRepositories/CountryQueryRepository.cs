using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryQueryRepository : EntityQueryRepository<Country, string>
    {
        public override (int, IEnumerable<Country>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Country>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Country> GetAll()
        {
            var result = Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Country>> GetAllAsync()
        {
            var result = await Query<Country>
                .Collection()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Country GetById(string countryCode)
        {
            var result = Query<Country>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetById]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Country> GetByIdAsync(string countryCode)
        {
            var result = await Query<Country>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCountry_GetById]")
                .Parameters(
                    p => p.Name("countryCode").Value(countryCode)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public Country GetCountryForCapitalCity(int capitalCityId)
        {
            var result = Query<Country>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetCountry]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(capitalCityId)
                )
                .Execute();

            return result.Record;
        }

        public async Task<Country> GetCountryForCapitalCityAsync(int capitalCityId)
        {
            var result = await Query<Country>
                .Single()
                .Connection(CountryWithCapitalCityConnectionClass.GetConnectionName())
                .StoredProcedure("[CountryBoundedContext].[pCapitalCity_GetCountry]")
                .Parameters(
                    p => p.Name("capitalCityId").Value(capitalCityId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Country>(new CountryQueryRepository());
        }

    }
}