using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class InsertCountryWithCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public CapitalCityEntity CapitalCity { get; private set; }

        public InsertCountryWithCapitalCityCommandAggregate(CountryWithCapitalCityDto countryWithCapitalCity)
            : base(new DataAccess.RepositoryContext("SqlServerTest.DomainFrameworkOneToOneTest.ConnectionString"))
        {
            RegisterCommandRepositoryFactory<CountryEntity>(() => new CountryCommandRepository());

            RegisterCommandRepositoryFactory<CapitalCityEntity>(() => new CapitalCityCommandRepository());

            RootEntity = new CountryEntity
            {
                Id = countryWithCapitalCity.CountryCode,
                Name = countryWithCapitalCity.Name
            };

            Enqueue(
                new InsertEntityCommandOperation<CountryEntity>(RootEntity)
            );

            if (countryWithCapitalCity.CapitalCity != null)
            {
                CapitalCity = new CapitalCityEntity
                {
                    Name = countryWithCapitalCity.CapitalCity.Name
                };

                Enqueue(
                    new AddLinkedEntityCommandOperation<CountryEntity, CapitalCityEntity>(
                        RootEntity, 
                        getLinkedEntity: () => CapitalCity
                    )
                );
            }
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}