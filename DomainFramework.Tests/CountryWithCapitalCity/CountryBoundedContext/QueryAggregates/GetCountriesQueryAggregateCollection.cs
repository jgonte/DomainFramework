using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class GetCountriesQueryAggregateCollection : GetQueryAggregateCollection<GetCountriesQueryAggregate, Country, CountryOutputDto>
    {
        public GetCountriesQueryAggregateCollection()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName());

            CountryQueryRepository.Register(context);

            CapitalCityQueryRepository.Register(context);

            RepositoryContext = context;
        }

    }
}