using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookWithPages.BookBoundedContext
{
    public class SaveBookCommandAggregate : CommandAggregate<Book>
    {
        public SaveBookCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
        }

        public SaveBookCommandAggregate(SaveBookInputDto book, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            Initialize(book, dependencies);
        }

        public override void Initialize(IInputDataTransferObject book, EntityDependency[] dependencies)
        {
            Initialize((SaveBookInputDto)book, dependencies);
        }

        private void Initialize(SaveBookInputDto book, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Book>(() => new BookCommandRepository());

            RegisterCommandRepositoryFactory<Page>(() => new PageCommandRepository());

            RootEntity = new Book
            {
                Id = book.Id,
                Title = book.Title,
                Category = book.Category,
                DatePublished = book.DatePublished,
                PublisherId = book.PublisherId
            };

            Enqueue(new SaveEntityCommandOperation<Book>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Book>(RootEntity, "Pages"));

            if (book.Pages?.Any() == true)
            {
                foreach (var page in book.Pages)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Book, Page>(RootEntity, () => new Page
                    {
                        Id = page.PageId,
                        Index = page.Index
                    }, "Pages"));
                }
            }
        }

    }
}