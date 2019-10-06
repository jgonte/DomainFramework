using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class DeleteEmployeePersonCommandAggregate : CommandAggregate<EmployeeEntity>
    {
        public DeleteEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, int? employeeId) :base(context)
        {
            RootEntity = new EmployeeEntity
            {
                Id = employeeId
            };

            Enqueue(
                new DeleteEntityCommandOperation<EmployeeEntity>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}