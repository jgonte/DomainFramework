using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ManagerEmployeesCommandAggregate : CommandAggregate<PersonEntity3>
    {
        public CommandCollectionEntityLink<PersonEntity3, PersonEntity3> EmployeesLink { get; set; } = new CommandCollectionEntityLink<PersonEntity3, PersonEntity3>();

        public IEnumerable<PersonEntity3> Employees => EmployeesLink.LinkedEntities;

        public ManagerEmployeesCommandAggregate(RepositoryContext context, PersonEntity3 entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<ICommandCollectionEntityLink<PersonEntity3>>();

            // Register the link to the employees collection
            CollectionEntityLinks.Add(EmployeesLink);
        }

        public void AddEmployee(PersonEntity3 employee)
        {
            EmployeesLink.AddEntity(employee);
        }
    }
}