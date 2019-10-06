using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class SaveManagerCommandAggregate : CommandAggregate<Manager>
    {
        public SaveManagerCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
        }

        public SaveManagerCommandAggregate(SaveManagerInputDto manager, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            Initialize(manager, dependencies);
        }

        public override void Initialize(IInputDataTransferObject manager, EntityDependency[] dependencies)
        {
            Initialize((SaveManagerInputDto)manager, dependencies);
        }

        private void Initialize(SaveManagerInputDto manager, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Manager>(() => new ManagerCommandRepository());

            RegisterCommandRepositoryFactory<Employee>(() => new EmployeeCommandRepository());

            RootEntity = new Manager
            {
                Id = manager.Id,
                Department = manager.Department,
                Name = manager.Name,
                SupervisorId = manager.SupervisorId
            };

            Enqueue(new SaveEntityCommandOperation<Manager>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Manager>(RootEntity, "Employees"));

            if (manager.Employees?.Any() == true)
            {
                foreach (var employee in manager.Employees)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Manager, Employee>(RootEntity, () => new Employee
                    {
                        Name = employee.Name
                    }, "Employees"));
                }
            }
        }

    }
}