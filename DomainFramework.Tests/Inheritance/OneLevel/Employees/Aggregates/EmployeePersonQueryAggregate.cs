using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeePersonQueryAggregate : QueryAggregate<EmployeeEntity, int?>
    {
        public GetEntityLoadOperation<PersonEntity> PersonLoadOperation { get; }

        public PersonEntity Person => PersonLoadOperation.Entity;

        public EmployeePersonQueryAggregate(RepositoryContext context) : base(context)
        {
            PersonLoadOperation = new GetEntityLoadOperation<PersonEntity>();

            LoadOperations.Enqueue(
                PersonLoadOperation
            );
        }       
    }
}