using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ExecutiveEmployeePersonQueryAggregate : QueryAggregate<int?, ExecutiveEntity>
    {
        public QueryInheritanceEmployeePersonEntityLink EmployeePersonLink { get; set; } = new QueryInheritanceEmployeePersonEntityLink();

        public PersonEntity Person => EmployeePersonLink.BaseEntity;

        public QueryInheritanceExecutiveEmployeePersonEntityLink ExecuteEmployeeLink { get; set; } = new QueryInheritanceExecutiveEmployeePersonEntityLink();

        public EmployeeEntity Employee => ExecuteEmployeeLink.BaseEntity;

        public ExecutiveEmployeePersonQueryAggregate(RepositoryContext context) : base(context)
        {
            // Create the links to the inheritance entity links
            InheritanceEntityLinks = new List<IQueryInheritanceEntityLink<int?>>();

            InheritanceEntityLinks.Add(EmployeePersonLink);

            InheritanceEntityLinks.Add(ExecuteEmployeeLink);
        }

    }
}