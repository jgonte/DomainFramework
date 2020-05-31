using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeeOutputDto : IOutputDataTransferObject
    {

    }

    class EmployeePersonQueryAggregate : GetByIdQueryAggregate<EmployeeEntity, int?, EmployeeOutputDto>
    {
        public GetEntityQueryOperation<PersonEntity> PersonLoadOperation { get; }

        public PersonEntity Person => PersonLoadOperation.Entity;

        public EmployeePersonQueryAggregate(RepositoryContext context) : base(context, null)
        {
            PersonLoadOperation = new GetEntityQueryOperation<PersonEntity>();

            QueryOperations.Enqueue(
                PersonLoadOperation
            );
        }

        public override void PopulateDto()
        {
        }
    }
}