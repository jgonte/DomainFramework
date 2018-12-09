using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class SaveExecutiveEmployeePersonCommandAggregate : CommandAggregate<ExecutiveEntity>
    {
        public PersonEntity Person { get; private set; }

        public EmployeeEntity Employee { get; private set; }

        public SaveExecutiveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, string firstName, decimal salary, decimal bonus) : base(context, null)
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

            TransactedOperations.Enqueue(
                new SaveEntityWithInheritanceTransactedOperation<ExecutiveEntity>(
                    RootEntity,
                    new SaveBaseEntityTransactedOperation<PersonEntity, EmployeeEntity>(Person),
                    new SaveBaseEntityTransactedOperation<EmployeeEntity, ExecutiveEntity>(Employee)
                )
            );
        }
    }
}