using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class CountryCapitalCityQueryAggregate : QueryAggregate<string, CountryEntity>
    {
        public CapitalCityEntity CapitalCity => CapitalCityLink.LinkedEntity;

        public CountryCapitalCityQueryAggregate(RepositoryContext context) : base(context)
        {
            // Create the links to the collection of entity links
            SingleEntityLinks = new List<IQuerySingleEntityLink>();

            // Register the link to the pages collection
            SingleEntityLinks.Add(CapitalCityLink);
        }

        public QuerySingleCapitalCityEntityLink CapitalCityLink { get; set; } = new QuerySingleCapitalCityEntityLink();
    }
}