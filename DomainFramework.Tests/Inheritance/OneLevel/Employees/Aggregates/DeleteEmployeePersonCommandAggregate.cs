using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteEmployeePersonCommandAggregate : CommandAggregate<EmployeeEntity>
    {
        public PersonEntity Person { get; private set; }

        public DeleteEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, int? employeeId) :base(context, null)
        {
            RootEntity = new EmployeeEntity
            {
                Id = employeeId
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<EmployeeEntity>(RootEntity, CommandOperationTypes.Delete)
            );
        }
    }
}