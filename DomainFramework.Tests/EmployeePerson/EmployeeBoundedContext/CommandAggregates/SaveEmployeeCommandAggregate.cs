using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class SaveEmployeeCommandAggregate : CommandAggregate<Employee>
    {
        public SaveEmployeeCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EmployeePersonConnectionClass.GetConnectionName()))
        {
        }

        public SaveEmployeeCommandAggregate(SaveEmployeeInputDto employee, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeePersonConnectionClass.GetConnectionName()))
        {
            Initialize(employee, dependencies);
        }

        public override void Initialize(IInputDataTransferObject employee, EntityDependency[] dependencies)
        {
            Initialize((SaveEmployeeInputDto)employee, dependencies);
        }

        private void Initialize(SaveEmployeeInputDto employee, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Employee>(() => new EmployeeCommandRepository());

            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            RootEntity = new Employee
            {
                Id = employee.EmployeeId,
                HireDate = employee.HireDate,
                Name = employee.Name,
                Gender = employee.Gender,
                CellPhone = (employee.CellPhone != null) ? new PhoneNumber
                {
                    AreaCode = employee.CellPhone.AreaCode,
                    Exchange = employee.CellPhone.Exchange,
                    Number = employee.CellPhone.Number
                } : null
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity, dependencies));
        }

    }
}