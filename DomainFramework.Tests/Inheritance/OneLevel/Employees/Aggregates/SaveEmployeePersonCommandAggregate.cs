using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class SaveEmployeePersonCommandAggregate : CommandAggregate<EmployeeEntity>
    {
        public PersonEntity Person { get; private set; }

        public SaveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, string firstName, int salary) :base(context, null)
        {
            Person = new PersonEntity
            {
                FirstName = firstName
            };

            RootEntity = new EmployeeEntity
            {
                Salary = salary
            };

            TransactedOperations.Enqueue(
                new SaveEntityWithInheritanceTransactedOperation<EmployeeEntity>(
                    RootEntity,
                    new SaveBaseEntityTransactedOperation<PersonEntity, EmployeeEntity>(Person)
                )
            );
        }
    }
}