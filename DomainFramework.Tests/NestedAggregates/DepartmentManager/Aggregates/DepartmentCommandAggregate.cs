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
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity
                            },
                            new EntityDependency
                            {
                                Entity = personEntity
                            }
                        })
                );

                if (employee.IsManager)
                {
                    var departmentManagerRoleEntity = new DepartmentManagerRoleEntity();

                    Enqueue(
                        new SaveEntityCommandOperation<DepartmentManagerRoleEntity>(
                            departmentManagerRoleEntity,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = employeeRoleEntity
                                },
                                new EntityDependency
                                {
                                    Entity = RootEntity
                                }
                            })
                    );
                }
            }
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}