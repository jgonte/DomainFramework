using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class CountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public CapitalCityEntity CapitalCity { get; private set; }

        public CountryCapitalCityCommandAggregate(DataAccess.RepositoryContext context, CountryWithCapitalCityDto countryWithCapitalCity) : base(context, null)
        {
            RootEntity = new CountryEntity(new CountryData
            {
                CountryCode = countryWithCapitalCity.CountryCode,
                Name = countryWithCapitalCity.Name
            });

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<CountryEntity>(RootEntity)
            );

            if (countryWithCapitalCity.CapitalCity != null)
            {
                CapitalCity = new CapitalCityEntity(new CapitalCityData { Name = countryWithCapitalCity.CapitalCity.Name });

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