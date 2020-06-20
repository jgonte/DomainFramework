using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class GetCountryByIdQueryAggregate : GetByIdQueryAggregate<Country, string, CountryOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<string, CapitalCity, CapitalCityOutputDto> GetCapitalCityLinkedAggregateQueryOperation { get; set; }

        public GetCountryByIdQueryAggregate() : this(null)
        {
        }

        public GetCountryByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(CountryWithCapitalCityConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            CountryQueryRepository.Register(context);

            CapitalCityQueryRepository.Register(context);

            GetCapitalCityLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<string, CapitalCity, CapitalCityOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("CapitalCity", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("CapitalCity", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((CapitalCityQueryRepository)repository).GetCapitalCityForCountry(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((CapitalCityQueryRepository)repository).GetCapitalCityForCountryAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is CapitalCity)
                    {
                        return new GetCapitalCityByIdQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetCapitalCityLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.CountryCode = RootEntity.Id;

            OutputDto.Name = RootEntity.Name;

            OutputDto.IsActive = RootEntity.IsActive;

            OutputDto.CapitalCity = GetCapitalCityLinkedAggregateQueryOperation.OutputDto;
        }

    }
}