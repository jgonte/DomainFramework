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

            var marriedToDependency = (Person)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Employee
            {
                Id = employee.EmployeeId,
                HireDate = employee.HireDate,
                Name = employee.Name,
                MarriedToPersonId = (marriedToDependency != null) ? marriedToDependency.Id : employee.MarriedToPersonId,
                CellPhone = (employee.CellPhone != null) ? new PhoneNumber
                {
                    AreaCode = employee.CellPhone.AreaCode,
                    Exchange = employee.CellPhone.Exchange,
                    Number = employee.CellPhone.Number
                } : null
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Employee>(RootEntity, "UnlinkSpouseFromPerson"));

            if (employee.Spouse != null)
            {
                ILinkedAggregateCommandOperation operation;

                var spouse = employee.Spouse;

                if (spouse is PersonInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Employee, SavePersonCommandAggregate, PersonInputDto>(
                        RootEntity,
                        (PersonInputDto)spouse,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Spouse"
                            }
                        }
                    );

                    Enqueue(operation);
                }
                else
                {
                    throw new NotImplementedException();
                }

                Enqueue(new UpdateEntityCommandOperation<Employee>(RootEntity, new EntityDependency[]
                {
                    new EntityDependency
                    {
                        Entity = operation.CommandAggregate.RootEntity,
                        Selector = "Spouse"
                    }
                }));
            }
        }

    }
}