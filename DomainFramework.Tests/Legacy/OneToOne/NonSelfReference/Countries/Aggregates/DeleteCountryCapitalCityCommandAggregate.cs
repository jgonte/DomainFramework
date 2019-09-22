using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteCountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public DeleteCountryCapitalCityCommandAggregate(string countryCode) 
            : base(new DataAccess.RepositoryContext("SqlServerTest.DomainFrameworkOneToOneTest.ConnectionString"))
        {
            RegisterCommandRepositoryFactory<CountryEntity>(() => new CountryCommandRepository());

            RootEntity = new CountryEntity
            {
                Id = countryCode
            };

            Enqueue(
                new DeleteEntityCommandOperation<CountryEntity>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}