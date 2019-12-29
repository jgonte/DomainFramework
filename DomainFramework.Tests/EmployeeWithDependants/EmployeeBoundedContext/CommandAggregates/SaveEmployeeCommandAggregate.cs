using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class SaveEmployeeCommandAggregate : CommandAggregate<Employee>
    {
        public SaveEmployeeCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()))
        {
        }

        public SaveEmployeeCommandAggregate(SaveEmployeeInputDto employee, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()))
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
                Id = employee.Id,
                HireDate = employee.HireDate,
                Name = employee.Name,
                ProviderEmployeeId = employee.ProviderEmployeeId,
                CellPhone = (employee.CellPhone != null) ? new PhoneNumber
                {
                    AreaCode = employee.CellPhone.AreaCode,
                    Exchange = employee.CellPhone.Exchange,
                    Number = employee.CellPhone.Number
                } : null
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Employee>(RootEntity, "Dependants"));

            if (employee.Dependants?.Any() == true)
            {
                foreach (var person in employee.Dependants)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Employee, Person>(RootEntity, () => new Person
                    {
                        Name = person.Name,
                        CellPhone = (person.CellPhone != null) ? new PhoneNumber
                        {
                            AreaCode = person.CellPhone.AreaCode,
                            Exchange = person.CellPhone.Exchange,
                            Number = person.CellPhone.Number
                        } : null
                    }, "Dependants"));
                }
            }
        }

    }
}