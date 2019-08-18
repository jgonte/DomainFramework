using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class SaveEmployeeCommandAggregate : CommandAggregate<Employee>
    {
        public SaveEmployeeCommandAggregate(SaveEmployeeInputDto employee) : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
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
                    Id = spouse.Id,
                    Name = spouse.Name,
                    MarriedToPersonId = spouse.MarriedToPersonId,
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
