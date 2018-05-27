using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class BookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        public BookPagesCommandAggregate(RepositoryContext context, BookEntity entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<ICommandCollectionEntityLink<BookEntity>>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(PagesLink);
        }

        public CommandCollectionEntityLink<BookEntity, PageEntity> PagesLink { get; set; } = new CommandCollectionEntityLink<BookEntity, PageEntity>();

        public IEnumerable<PageEntity> Pages => PagesLink.LinkedEntities;

        public void AddPage(PageEntity pageEntity)
        {
            PagesLink.AddEntity(pageEntity);
        }

        public void Delete()
        {
            var repository = RepositoryContext.GetCommandRepository(RootEntity.GetType());

            repository.Delete(RootEntity);
        }
    }
}
