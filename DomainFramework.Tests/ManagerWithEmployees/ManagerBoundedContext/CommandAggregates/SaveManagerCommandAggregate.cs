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

            var managerDependency = (Manager)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Manager
            {
                Id = manager.ManagerId,
                Department = manager.Department,
                Name = manager.Name,
                SupervisorId = (managerDependency != null) ? managerDependency.Id : manager.SupervisorId
            };

            Enqueue(new SaveEntityCommandOperation<Manager>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Manager>(RootEntity, "UnlinkEmployeesFromManager"));

            if (manager.Employees?.Any() == true)
            {
                foreach (var dto in manager.Employees)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is EmployeeInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Manager, SaveEmployeeCommandAggregate, EmployeeInputDto>(
                            RootEntity,
                            (EmployeeInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Employees"
                                }
                            }
                        );

                        Enqueue(operation);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

    }
}