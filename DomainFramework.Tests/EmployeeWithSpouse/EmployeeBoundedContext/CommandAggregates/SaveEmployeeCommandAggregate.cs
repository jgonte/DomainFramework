using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class SaveEmployeeCommandAggregate : CommandAggregate<Employee>
    {
        public SaveEmployeeCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName()))
        {
        }

        public SaveEmployeeCommandAggregate(SaveEmployeeInputDto employee, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName()))
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
                MarriedToPersonId = employee.MarriedToPersonId,
                CellPhone = (employee.CellPhone != null) ? new PhoneNumber
                {
                    AreaCode = employee.CellPhone.AreaCode,
                    Exchange = employee.CellPhone.Exchange,
                    Number = employee.CellPhone.Number
                } : null
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Employee>(RootEntity, "Spouse"));

            if (employee.Spouse != null)
            {
                var spouse = employee.Spouse;

                var entityForSpouse = new Person
                {
                    Name = spouse.Name,
                    CellPhone = (spouse.CellPhone != null) ? new PhoneNumber
                    {
                        AreaCode = spouse.CellPhone.AreaCode,
                        Exchange = spouse.CellPhone.Exchange,
                        Number = spouse.CellPhone.Number
                    } : null
                };

                Enqueue(new AddLinkedEntityCommandOperation<Employee, Person>(RootEntity, () => entityForSpouse, "Spouse"));

                Enqueue(new UpdateEntityCommandOperation<Employee>(RootEntity, new EntityDependency[]
                {
                    new EntityDependency
                    {
                        Entity = entityForSpouse,
                        Selector = "Spouse"
                    }
                }));
            }
        }

    }
}