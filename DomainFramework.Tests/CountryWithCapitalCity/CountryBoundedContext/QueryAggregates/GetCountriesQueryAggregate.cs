using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class GetCountriesQueryAggregate : QueryAggregateCollection<Country, CountryOutputDto, GetCountryByIdQueryAggregate>
    {
        public GetCountriesQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            CountryQueryRepository.Register(context);

            CapitalCityQueryRepository.Register(context);
        }

        public (int, IEnumerable<CountryOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (CountryQueryRepository)RepositoryContext.GetQueryRepository(typeof(Country));

            var (count, entities) = repository.Get(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<CountryOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (CountryQueryRepository)RepositoryContext.GetQueryRepository(typeof(Country));

            var (count, entities) = await repository.GetAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}