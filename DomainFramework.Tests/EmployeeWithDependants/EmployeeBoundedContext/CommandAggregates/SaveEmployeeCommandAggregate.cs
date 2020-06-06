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

            var employeeDependency = (Employee)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Employee
            {
                Id = employee.EmployeeId,
                HireDate = employee.HireDate,
                Name = employee.Name,
                ProviderEmployeeId = (employeeDependency != null) ? employeeDependency.Id : employee.ProviderEmployeeId,
                CellPhone = new PhoneNumber
                {
                    AreaCode = employee.CellPhone.AreaCode,
                    Exchange = employee.CellPhone.Exchange,
                    Number = employee.CellPhone.Number
                }
            };

            Enqueue(new SaveEntityCommandOperation<Employee>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Employee>(RootEntity, "UnlinkDependantsFromEmployee"));

            if (employee.Dependants?.Any() == true)
            {
                foreach (var dto in employee.Dependants)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is PersonInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Employee, SavePersonCommandAggregate, PersonInputDto>(
                            RootEntity,
                            (PersonInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Dependants"
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