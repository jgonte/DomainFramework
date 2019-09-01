using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveEmployeePersonQueryAggregate : GetByIdQueryAggregate<ExecutiveEntity, int?, object>
    {
        public GetEntityQueryOperation<PersonEntity> PersonLoadOperation { get; }

        public PersonEntity Person => PersonLoadOperation.Entity;

        public GetEntityQueryOperation<EmployeeEntity> EmployeeLoadOperation { get; }

        public EmployeeEntity Employee => EmployeeLoadOperation.Entity;       

        public ExecutiveEmployeePersonQueryAggregate(RepositoryContext context) : base(context)
        {
            PersonLoadOperation = new GetEntityQueryOperation<PersonEntity>();

            LoadOperations.Enqueue(
                PersonLoadOperation
            );

            EmployeeLoadOperation = new GetEntityQueryOperation<EmployeeEntity>();

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