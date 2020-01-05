using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class GetCountriesQueryAggregate : GetQueryAggregateCollection<Country, CountryOutputDto, GetCountryByIdQueryAggregate>
    {
        public GetCountriesQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            CountryQueryRepository.Register(context);
        }

    }
}