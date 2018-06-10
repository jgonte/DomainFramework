using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public CapitalCityEntity CapitalCity { get; private set; }

        public CountryCapitalCityCommandAggregate(DataAccess.RepositoryContext context, string countryCode, string countryName, string capitalCity) : base(context, null)
        {
            RootEntity = new CountryEntity(new CountryData
            {
                CountryCode = countryCode,
                Name = countryName
            });

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<CountryEntity>(RootEntity)
            );

            if (!string.IsNullOrEmpty(capitalCity))
            {
                CapitalCity = new CapitalCityEntity(new CapitalCityData { Name = capitalCity });

                TransactedSaveOperations.Enqueue(
                    new SingleEntityLinkTransactedOperation<CountryEntity, CapitalCityEntity>(RootEntity, CapitalCity)
                );
            }

            TransactedDeleteOperations.Enqueue(
                new DeleteEntityTransactedOperation<CountryEntity>(RootEntity)
            );
        }
    }
}