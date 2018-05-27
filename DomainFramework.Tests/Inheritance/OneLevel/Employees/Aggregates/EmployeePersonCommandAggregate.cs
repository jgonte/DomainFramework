using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests.OneLevelInheritance
{
    class EmployeePersonCommandAggregate : CommandAggregate<EmployeeEntity>
    {
        public EmployeePersonCommandAggregate(Core.RepositoryContext context, EmployeeEntity entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            InheritanceEntityLinks = new List<ICommandInheritanceEntityLink<EmployeeEntity>>();

            // Register the link to the pages collection
            InheritanceEntityLinks.Add(PersonLink);
        }

        public CommandInheritanceEntityLink<EmployeeEntity, PersonEntity> PersonLink { get; set; } = new CommandInheritanceEntityLink<EmployeeEntity, PersonEntity>();

        public PersonEntity Person => PersonLink.LinkedEntity;

        public void SetPerson(PersonEntity personEntity)
        {
            PersonLink.SetEntity(personEntity);
        }
    }
}