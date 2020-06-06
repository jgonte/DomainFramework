using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class SaveEmployeeCommandAggregate : CommandAggregate<Employee>
    {
        public SaveEmployeeCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
        }

        public SaveEmployeeCommandAggregate(EmployeeInputDto employee, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            Initialize(employee, dependencies);
        }

        public override void Initialize(IInputDataTransferObject employee, EntityDependency[] dependencies)
        {
            Initialize((EmployeeInputDto)employee, dependencies);
        }

        private void Initialize(EmployeeInputDto employee, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Employee>(() => new EmployeeCommandRepository());

            var managerDependency = (Manager)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Employee
            {
                Id = employee.EmployeeId,
                Name = employee.Name,
                SupervisorId = (managerDependency != null) ? managerDependency.Id : employee.SupervisorId
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity, dependencies));
        }

    }
}