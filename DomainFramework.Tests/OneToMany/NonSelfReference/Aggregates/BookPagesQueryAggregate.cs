using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class BookPagesQueryAggregate : QueryAggregate<int?, BookEntity>
    {
        public QueryCollectionPageEntityLink PagesLink { get; set; } = new QueryCollectionPageEntityLink();

        public IEnumerable<PageEntity> Pages => PagesLink.LinkedEntities;

        public BookPagesQueryAggregate() : this(null)
        {
        }

        public BookPagesQueryAggregate(RepositoryContext context) : base(context)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<IQueryCollectionEntityLink>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(PagesLink);
        }
    }
}