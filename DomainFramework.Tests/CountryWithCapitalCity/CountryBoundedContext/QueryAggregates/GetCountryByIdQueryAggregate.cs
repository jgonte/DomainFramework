using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class GetCountryByIdQueryAggregate : GetByIdQueryAggregate<Country, string, CountryOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<CapitalCity> GetCapitalCityQueryOperation { get; }

        public CapitalCity CapitalCity => GetCapitalCityQueryOperation.LinkedEntity;

        public GetCountryByIdQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName());

            CountryQueryRepository.Register(context);

            CapitalCityQueryRepository.Register(context);

            RepositoryContext = context;

            GetCapitalCityQueryOperation = new GetSingleLinkedEntityQueryOperation<CapitalCity>
            {
                GetLinkedEntity = (repository, entity, user) => ((CapitalCityQueryRepository)repository).GetCapitalCityForCountry(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((CapitalCityQueryRepository)repository).GetCapitalCityForCountryAsync(RootEntity.Id)
            };

            QueryOperations.Enqueue(GetCapitalCityQueryOperation);
        }

        public CapitalCityOutputDto GetCapitalCityDto()
        {
            if (CapitalCity != null)
            {
                var dto = new CapitalCityOutputDto
                {
                    Id = CapitalCity.Id.Value,
                    Name = CapitalCity.Name,
                    CountryCode = CapitalCity.CountryCode
                };

                return dto;
            }

            return null;
        }

        public override void PopulateDto(Country entity)
        {
            OutputDto.Id = entity.Id;

            OutputDto.Name = entity.Name;

            OutputDto.IsActive = entity.IsActive;

            OutputDto.CapitalCity = GetCapitalCityDto();
        }

    }
}