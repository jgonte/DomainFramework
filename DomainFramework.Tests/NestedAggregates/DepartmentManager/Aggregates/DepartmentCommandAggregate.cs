using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DepartmentCommandAggregate : CommandAggregate<DepartmentEntity>
    {
        public DepartmentCommandAggregate(DataAccess.RepositoryContext context, DepartmentDto department) : base(context)
        {
            RootEntity = new DepartmentEntity
            {
                Name = department.Name
            };

            Enqueue(new SaveEntityCommandOperation<DepartmentEntity>(RootEntity));

            foreach (var employee in department.Employees)
            {
                // Create the employee
                var personEntity = new PersonEntity
                {
                    FirstName = employee.FirstName
                };

                Enqueue(
                    new SaveEntityCommandOperation<PersonEntity>(personEntity)
                );

                // Set the salary and department id
                var employeeRoleEntity = new EmployeeRoleEntity
                {
                    Salary = employee.Salary
                };

                Enqueue(
                    new SaveEntityCommandOperation<EmployeeRoleEntity>(
                        employeeRoleEntity,
                        new IEntity[] { RootEntity, personEntity }
                    )
                );

                if (employee.IsManager)
                {
                    var departmentManagerRoleEntity = new DepartmentManagerRoleEntity();

                    Enqueue(
                        new SaveEntityCommandOperation<DepartmentManagerRoleEntity>(
                            departmentManagerRoleEntity,
                            new IEntity[] { employeeRoleEntity, RootEntity })
                    );
                }
            }
        }
    }
}