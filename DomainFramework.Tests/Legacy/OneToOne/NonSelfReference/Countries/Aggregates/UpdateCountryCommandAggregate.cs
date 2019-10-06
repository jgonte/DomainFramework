using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class UpdateCountryCommandAggregate : CommandAggregate<CountryEntity>
    {
        public CapitalCityEntity CapitalCity { get; private set; }

        public UpdateCountryCommandAggregate(CountryWithCapitalCityDto countryWithCapitalCity) 
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
                new UpdateEntityCommandOperation<CountryEntity>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}