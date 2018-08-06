using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class DepartmentCommandAggregate : CommandAggregate<DepartmentEntity>
    {
        public DepartmentCommandAggregate(DataAccess.RepositoryContext context, DepartmentEntity entity) : base(context, entity)
        {
            TransactedSaveOperations.Enqueue(new SaveEntityTransactedOperation<DepartmentEntity>(entity));
        }

        public EmployeeRoleEntity AddEmployee(string firstName, int salary)
        {
            // Create the person
            var personEntity = new PersonEntity
            {
                FirstName = firstName
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<PersonEntity>(personEntity)
            );

            var employeeRoleEntity = new EmployeeRoleEntity
            {
                Salary = salary
            };

            TransactedSaveOperations.Enqueue(
                new SaveBinaryEntityTransactedOperation<EmployeeRoleEntity, DepartmentEntity, PersonEntity>(employeeRoleEntity, RootEntity, personEntity)
            );

            return employeeRoleEntity;
        }

        public void SetManager(EmployeeRoleEntity employeeRoleEntity)
        {
            var departmentManagerRoleEntity = new DepartmentManagerRoleEntity();

            TransactedSaveOperations.Enqueue(
                new SaveBinaryEntityTransactedOperation<DepartmentManagerRoleEntity, EmployeeRoleEntity, DepartmentEntity>(departmentManagerRoleEntity, employeeRoleEntity, RootEntity)
            );
        }
    }
}