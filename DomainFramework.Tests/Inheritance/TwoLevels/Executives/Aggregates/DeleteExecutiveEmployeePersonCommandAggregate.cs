using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteExecutiveEmployeePersonCommandAggregate : CommandAggregate<ExecutiveEntity>
    {
        public DeleteExecutiveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, int? executiveId) : base(context, null)
        {
            RootEntity = new ExecutiveEntity
            {
                Id = executiveId
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<ExecutiveEntity>(RootEntity, CommandOperations.Delete)
            );
        }
    }
}