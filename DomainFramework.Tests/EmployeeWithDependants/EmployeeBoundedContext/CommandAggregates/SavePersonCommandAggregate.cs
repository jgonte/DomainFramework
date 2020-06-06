using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(PersonInputDto person, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()))
        {
            Initialize(person, dependencies);
        }

        public override void Initialize(IInputDataTransferObject person, EntityDependency[] dependencies)
        {
            Initialize((PersonInputDto)person, dependencies);
        }

        private void Initialize(PersonInputDto person, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            var employeeDependency = (Employee)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Person
            {
                Id = person.PersonId,
                Name = person.Name,
                ProviderEmployeeId = (employeeDependency != null) ? employeeDependency.Id : person.ProviderEmployeeId,
                CellPhone = new PhoneNumber
                {
                    AreaCode = person.CellPhone.AreaCode,
                    Exchange = person.CellPhone.Exchange,
                    Number = person.CellPhone.Number
                }
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));
        }

    }
}