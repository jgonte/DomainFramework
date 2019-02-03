using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteCountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public DeleteCountryCapitalCityCommandAggregate(string countryCode)
        {
            RepositoryContext = new DataAccess.RepositoryContext("SqlServerTest.DomainFrameworkOneToOneTest.ConnectionString");

            RepositoryContext.RegisterCommandRepositoryFactory<CountryEntity>(() => new CountryCommandRepository());

            RootEntity = new CountryEntity
            {
                Id = countryCode
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<CountryEntity>(RootEntity, CommandOperations.Delete)
            );
        }
    }
}