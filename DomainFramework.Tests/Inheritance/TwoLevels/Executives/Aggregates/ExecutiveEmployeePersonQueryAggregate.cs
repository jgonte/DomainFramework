using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveEmployeePersonQueryAggregate : QueryAggregate<ExecutiveEntity, int?>
    {
        public GetEntityLoadOperation<PersonEntity> PersonLoadOperation { get; }

        public PersonEntity Person => PersonLoadOperation.Entity;

        public GetEntityLoadOperation<EmployeeEntity> EmployeeLoadOperation { get; }

        public EmployeeEntity Employee => EmployeeLoadOperation.Entity;       

        public ExecutiveEmployeePersonQueryAggregate(RepositoryContext context) : base(context)
        {
            PersonLoadOperation = new GetEntityLoadOperation<PersonEntity>();

            LoadOperations.Enqueue(
                PersonLoadOperation
            );

            EmployeeLoadOperation = new GetEntityLoadOperation<EmployeeEntity>();

            LoadOperations.Enqueue(
                EmployeeLoadOperation
            );
        }

    }
}