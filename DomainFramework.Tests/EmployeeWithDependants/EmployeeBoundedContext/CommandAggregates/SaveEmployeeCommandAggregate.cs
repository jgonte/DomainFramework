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

        public SaveEmployeeCommandAggregate(SaveEmployeeInputDto employee) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()))
        {
            Initialize(employee);
        }

        public override void Initialize(IInputDataTransferObject employee)
        {
            Initialize((SaveEmployeeInputDto)employee);
        }

        private void Initialize(SaveEmployeeInputDto employee)
        {
            RegisterCommandRepositoryFactory<Employee>(() => new EmployeeCommandRepository());

            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            RootEntity = new Employee
            {
                Id = employee.Id,
                HireDate = employee.HireDate,
                Name = employee.Name,
                ProviderEmployeeId = employee.ProviderEmployeeId,
                CellPhone = new PhoneNumber
                {
                    AreaCode = employee.CellPhone.AreaCode,
                    Exchange = employee.CellPhone.Exchange,
                    Number = employee.CellPhone.Number
                }
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Employee>(RootEntity, "Dependants"));

            if (employee.Dependants?.Any() == true)
            {
                foreach (var dto in employee.Dependants)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Employee, Person>(RootEntity, () => new Person
                    {
                        Name = dto.Name,
                        ProviderEmployeeId = dto.ProviderEmployeeId,
                        CellPhone = new PhoneNumber
                        {
                            AreaCode = dto.CellPhone.AreaCode,
                            Exchange = dto.CellPhone.Exchange,
                            Number = dto.CellPhone.Number
                        }
                    }, "Dependants"));
                }
            }
        }

    }
}