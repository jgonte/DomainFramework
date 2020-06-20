using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class GetCapitalCityByIdQueryAggregate : GetByIdQueryAggregate<CapitalCity, int?, CapitalCityOutputDto>
    {
        public GetCapitalCityByIdQueryAggregate() : this(null)
        {
        }

        public GetCapitalCityByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            CapitalCityQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.CapitalCityId = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.CountryCode = RootEntity.CountryCode;
        }

    }
}