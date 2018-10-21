using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryCapitalCityQueryAggregate : QueryAggregate<CountryEntity, string, object>
    {
        public GetSingleLinkedEntityLoadOperation<CapitalCityEntity> GetCapitalCityLoadOperation { get; }

        public CapitalCityEntity CapitalCity => GetCapitalCityLoadOperation.LinkedEntity;

        public CountryCapitalCityQueryAggregate(RepositoryContext context) : base(context)
        {
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
            throw new System.NotImplementedException();
        }
    }
}