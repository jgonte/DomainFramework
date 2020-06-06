using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(PersonInputDto person, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName()))
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

            var marriedToDependency = (Person)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Person
            {
                Id = person.PersonId,
                Name = person.Name,
                MarriedToPersonId = (marriedToDependency != null) ? marriedToDependency.Id : person.MarriedToPersonId,
                CellPhone = (person.CellPhone != null) ? new PhoneNumber
                {
                    AreaCode = person.CellPhone.AreaCode,
                    Exchange = person.CellPhone.Exchange,
                    Number = person.CellPhone.Number
                } : null
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));
        }

    }
}