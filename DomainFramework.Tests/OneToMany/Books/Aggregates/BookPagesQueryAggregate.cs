using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class BookPagesQueryAggregate : QueryAggregate<int?, BookEntity>
    {
        public IEnumerable<PageEntity> Pages => PagesLink.LinkedEntities;

        public BookPagesQueryAggregate(RepositoryContext context, BookEntity entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<IQueryCollectionEntityLink>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(PagesLink);
        }

        public QueryCollectionPageEntityLink PagesLink { get; set; } = new QueryCollectionPageEntityLink();

    }
}