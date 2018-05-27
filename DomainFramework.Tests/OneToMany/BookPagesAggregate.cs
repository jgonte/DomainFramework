using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class BookPagesAggregate : CommandAggregate<BookEntity>
    {
        public BookPagesAggregate(RepositoryContext context, BookEntity entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<ICollectionEntityLink>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(PagesLink);
        }

        public CollectionPageEntityLink PagesLink { get; set; } = new CollectionPageEntityLink();

        public IEnumerable<PageEntity> Pages => PagesLink.LinkedEntities;

        public void AddPage(PageEntity pageEntity)
        {
            PagesLink.AddEntity(pageEntity);
        }
    }
}
