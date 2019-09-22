using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class ExecutiveOutputDto : IOutputDataTransferObject
    {

    }

    class ExecutiveEmployeePersonQueryAggregate : GetByIdQueryAggregate<ExecutiveEntity, int?, ExecutiveOutputDto>
    {
        public GetEntityQueryOperation<PersonEntity> PersonLoadOperation { get; }

        public PersonEntity Person => PersonLoadOperation.Entity;

        public GetEntityQueryOperation<EmployeeEntity> EmployeeLoadOperation { get; }

        public EmployeeEntity Employee => EmployeeLoadOperation.Entity;       

        public ExecutiveEmployeePersonQueryAggregate(RepositoryContext context) : base(context)
        {
            PersonLoadOperation = new GetEntityQueryOperation<PersonEntity>();

            QueryOperations.Enqueue(
                PersonLoadOperation
            );

            EmployeeLoadOperation = new GetEntityQueryOperation<EmployeeEntity>();

            QueryOperations.Enqueue(
                EmployeeLoadOperation
            );
        }

        public override void PopulateDto(ExecutiveEntity entity)
        {
        }
    }
}