using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeePersonCommandAggregate : CommandAggregate<EmployeeEntity>
    {
        public PersonEntity Person { get; private set; }

        public EmployeePersonCommandAggregate(DataAccess.RepositoryContext context, string firstName, int salary) :base(context, null)
        {
            Person = new PersonEntity
            {
                FirstName = firstName
            };

            RootEntity = new EmployeeEntity
            {
                Salary = salary
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityWithInheritanceTransactedOperation<EmployeeEntity>(
                    RootEntity,
                    new SaveBaseEntityTransactedOperation<PersonEntity, EmployeeEntity>(Person)
                )
            );

            TransactedDeleteOperations.Enqueue(
                new DeleteEntityTransactedOperation<EmployeeEntity>(RootEntity)
            );
        }
    }
}