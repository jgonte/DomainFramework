using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveEmployeePersonCommandAggregate : CommandAggregate<ExecutiveEntity>
    {
        public PersonEntity Person { get; private set; }

        public EmployeeEntity Employee { get; private set; }

        public ExecutiveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, string firstName, decimal salary, decimal bonus) : base(context, null)
        {
            Person = new PersonEntity
            {
                FirstName = firstName
            };

            Employee = new EmployeeEntity
            {
                Salary = salary
            };

            RootEntity = new ExecutiveEntity
            {
                Bonus = bonus
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityWithInheritanceTransactedOperation<ExecutiveEntity>(
                    RootEntity,
                    new SaveBaseEntityTransactedOperation<PersonEntity, EmployeeEntity>(Person),
                    new SaveBaseEntityTransactedOperation<EmployeeEntity, ExecutiveEntity>(Employee)
                )
            );

            TransactedDeleteOperations.Enqueue(
                new DeleteEntityTransactedOperation<ExecutiveEntity>(RootEntity)
            );
        }
    }
}