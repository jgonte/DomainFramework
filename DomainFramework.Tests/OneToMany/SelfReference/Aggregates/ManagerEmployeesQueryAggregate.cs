using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ManagerEmployeesQueryAggregate : QueryAggregate<int?, PersonEntity3>
    {
        public QueryCollectionEmployeeEntityLink EmployeesLink { get; set; } = new QueryCollectionEmployeeEntityLink();

        public IEnumerable<PersonEntity3> Employees => EmployeesLink.LinkedEntities;

        public ManagerEmployeesQueryAggregate() : this(null)
        {
        }

        public ManagerEmployeesQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<IQueryCollectionEntityLink>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(EmployeesLink);
        }
    }
}