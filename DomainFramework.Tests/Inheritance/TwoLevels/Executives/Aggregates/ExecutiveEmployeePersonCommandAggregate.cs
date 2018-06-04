using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ExecutiveEmployeePersonCommandAggregate : CommandAggregate<ExecutiveEntity>
    {
        public CommandInheritanceEntityLink<EmployeeEntity, PersonEntity> EmployeePersonLink { get; set; } = new CommandInheritanceEntityLink<EmployeeEntity, PersonEntity>();

        public PersonEntity Person => EmployeePersonLink.BaseEntity;

        public CommandInheritanceEntityLink<ExecutiveEntity, EmployeeEntity> ExecutiveEmployeeLink { get; set; } = new CommandInheritanceEntityLink<ExecutiveEntity, EmployeeEntity>();

        public EmployeeEntity Employee => ExecutiveEmployeeLink.BaseEntity;

        public ExecutiveEmployeePersonCommandAggregate(RepositoryContext context, ExecutiveEntity entity) : base(context, entity)
        {
            // Create the links to the collection of inheritance entity links
            InheritanceEntityLinks = new List<ICommandInheritanceEntityLink>();

            // Register the links from base to derived order
            InheritanceEntityLinks.Add(EmployeePersonLink);

            InheritanceEntityLinks.Add(ExecutiveEmployeeLink);
        }

        public void SetPerson(PersonEntity personEntity)
        {
            EmployeePersonLink.SetBaseEntity(personEntity);
        }

        public void SetEmployee(EmployeeEntity employeeEntity)
        {
            ExecutiveEmployeeLink.SetBaseEntity(employeeEntity);
        }
    }
}