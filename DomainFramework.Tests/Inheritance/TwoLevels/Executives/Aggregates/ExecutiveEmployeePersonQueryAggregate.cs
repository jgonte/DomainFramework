using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveEmployeePersonQueryAggregate : GetByIdQueryAggregate<ExecutiveEntity, int?, object>
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

        public override object GetDataTransferObject()
        {
            return null;
        }
    }
}