using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryCapitalCityQueryAggregate : GetByIdQueryAggregate<CountryEntity, string, object>
    {
        public GetSingleLinkedEntityLoadOperation<CapitalCityEntity> GetCapitalCityLoadOperation { get; }

        public CapitalCityEntity CapitalCity => GetCapitalCityLoadOperation.LinkedEntity;

        public CountryCapitalCityQueryAggregate()
        {
            RepositoryContext = new DataAccess.RepositoryContext("SqlServerTest.DomainFrameworkOneToOneTest.ConnectionString");

            RepositoryContext.RegisterQueryRepository<CountryEntity>(new CountryQueryRepository());

            RepositoryContext.RegisterQueryRepository<CapitalCityEntity>(new CapitalCityQueryRepository());

            GetCapitalCityLoadOperation = new GetSingleLinkedEntityLoadOperation<CapitalCityEntity>
            {
                GetLinkedEntity = (repository, entity, user) =>
                    ((CapitalCityQueryRepository)repository).GetForCountry(RootEntity.Id, user),

                GetLinkedEntityAsync = async (repository, entity, user) =>
                    await ((CapitalCityQueryRepository)repository).GetForCountryAsync(RootEntity.Id, user)
            };

            LoadOperations.Enqueue(
                GetCapitalCityLoadOperation
            );
        }

        public override object GetDataTransferObject()
        {
            return null;
        }
    }
}