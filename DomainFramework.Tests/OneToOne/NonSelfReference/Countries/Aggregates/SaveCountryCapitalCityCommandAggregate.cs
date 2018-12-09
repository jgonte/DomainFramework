using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class SaveCountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public CapitalCityEntity CapitalCity { get; private set; }

        public SaveCountryCapitalCityCommandAggregate(DataAccess.RepositoryContext context, CountryWithCapitalCityDto countryWithCapitalCity) : base(context, null)
        {
            RootEntity = new CountryEntity(new CountryData
            {
                CountryCode = countryWithCapitalCity.CountryCode,
                Name = countryWithCapitalCity.Name
            });

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<CountryEntity>(RootEntity, CommandOperationTypes.Save)
            );

            if (countryWithCapitalCity.CapitalCity != null)
            {
                CapitalCity = new CapitalCityEntity(new CapitalCityData { Name = countryWithCapitalCity.CapitalCity.Name });

                TransactedOperations.Enqueue(
                    new SingleEntityLinkTransactedOperation<CountryEntity, CapitalCityEntity>(RootEntity, CapitalCity)
                );
            }
        }
    }
}