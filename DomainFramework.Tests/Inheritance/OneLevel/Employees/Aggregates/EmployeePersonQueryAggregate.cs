using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class EmployeePersonQueryAggregate : QueryAggregate<int?, EmployeeEntity>
    {
        public QueryInheritanceEmployeePersonEntityLink EmployeePersonLink { get; set; } = new QueryInheritanceEmployeePersonEntityLink();

        public PersonEntity Person => EmployeePersonLink.BaseEntity;

        public EmployeePersonQueryAggregate(RepositoryContext context) : base(context)
        {
            // Create the links to the inheritance entity links
            InheritanceEntityLinks = new List<IQueryInheritanceEntityLink<int?>>();

            // Register the link to the pages collection
            InheritanceEntityLinks.Add(EmployeePersonLink);
        }       
    }
}