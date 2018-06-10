using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class PersonSpouseQueryAggregate : QueryAggregate<int?, PersonEntity2>
    {
        public QuerySingleSpouseEntityLink SpouseLink { get; set; } = new QuerySingleSpouseEntityLink();

        public PersonEntity2 Spouse => SpouseLink.LinkedEntity;

        public PersonSpouseQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            // Create the links to the collection of entity links
            SingleEntityLinks = new List<IQuerySingleEntityLink>();

            // Register the link to the pages collection
            SingleEntityLinks.Add(SpouseLink);
        }

    }
}