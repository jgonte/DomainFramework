using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryOutputDto : IOutputDataTransferObject
    {

    }

    class CountryCapitalCityQueryAggregate : GetByIdQueryAggregate<CountryEntity, string, CountryOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<CapitalCityEntity> GetCapitalCityLoadOperation { get; }

        public CapitalCityEntity CapitalCity => GetCapitalCityLoadOperation.LinkedEntity;

        public CountryCapitalCityQueryAggregate()
        {
            RepositoryContext = new DataAccess.RepositoryContext("SqlServerTest.DomainFrameworkOneToOneTest.ConnectionString");

            RepositoryContext.RegisterQueryRepository<CountryEntity>(new CountryQueryRepository());

            RepositoryContext.RegisterQueryRepository<CapitalCityEntity>(new CapitalCityQueryRepository());

            GetCapitalCityLoadOperation = new GetSingleLinkedEntityQueryOperation<CapitalCityEntity>
            {
                GetLinkedEntity = (repository, entity, user) =>
                    ((CapitalCityQueryRepository)repository).GetForCountry(RootEntity.Id, user),

                GetLinkedEntityAsync = async (repository, entity, user) =>
                    await ((CapitalCityQueryRepository)repository).GetForCountryAsync(RootEntity.Id, user)
            };

            QueryOperations.Enqueue(
                GetCapitalCityLoadOperation
            );
        }

        public override void PopulateDto(CountryEntity entity)
        {
        }
    }
}