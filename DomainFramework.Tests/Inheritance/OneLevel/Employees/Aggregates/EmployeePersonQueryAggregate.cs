using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeePersonQueryAggregate : QueryAggregate<EmployeeEntity, int?, object>
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

        public override object GetDataTransferObject()
        {
            throw new System.NotImplementedException();
        }
    }
}