using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteCountryCapitalCityCommandAggregate : CommandAggregate<CountryEntity>
    {
        public DeleteCountryCapitalCityCommandAggregate(DataAccess.RepositoryContext context, string countryCode) : base(context, null)
        {
            RootEntity = new CountryEntity
            {
                Id = countryCode
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<CountryEntity>(RootEntity, CommandOperationTypes.Delete)
            );
        }
    }
}