using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DepartmentCommandAggregate : CommandAggregate<DepartmentEntity>
    {
        public DepartmentCommandAggregate(DataAccess.RepositoryContext context, DepartmentEntity entity) : base(context, entity)
        {
            TransactedOperations.Enqueue(new EntityCommandTransactedOperation<DepartmentEntity>(entity, CommandOperationTypes.Save));
        }

        public EmployeeRoleEntity AddEmployee(string firstName, int salary)
        {
            // Create the person
            var personEntity = new PersonEntity
            {
                FirstName = firstName
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<PersonEntity>(personEntity, CommandOperationTypes.Save)
            );

            var employeeRoleEntity = new EmployeeRoleEntity
            {
                Salary = salary
            };

            TransactedOperations.Enqueue(
                new SaveBinaryEntityTransactedOperation<EmployeeRoleEntity, DepartmentEntity, PersonEntity>(employeeRoleEntity, RootEntity, personEntity)
            );

            return employeeRoleEntity;
        }

        public void SetManager(EmployeeRoleEntity employeeRoleEntity)
        {
            var departmentManagerRoleEntity = new DepartmentManagerRoleEntity();

            TransactedOperations.Enqueue(
                new SaveBinaryEntityTransactedOperation<DepartmentManagerRoleEntity, EmployeeRoleEntity, DepartmentEntity>(departmentManagerRoleEntity, employeeRoleEntity, RootEntity)
            );
        }
    }
}