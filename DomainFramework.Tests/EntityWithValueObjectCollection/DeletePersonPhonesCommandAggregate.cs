using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class DeletePersonPhonesCommandAggregate : CommandAggregate<PersonEntity4>
    {
        public DeletePersonPhonesCommandAggregate(DataAccess.RepositoryContext context, int personId) : base(context, null)
        {
            RootEntity = new PersonEntity4
            {
                Id = personId
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<PersonEntity4>(RootEntity, CommandOperations.Delete)
            );
        }
    }
}