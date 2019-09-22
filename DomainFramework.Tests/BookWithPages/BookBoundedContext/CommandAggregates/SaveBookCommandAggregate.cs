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

        public SaveBookCommandAggregate(SaveBookInputDto book) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            Initialize(book);
        }

        public override void Initialize(IInputDataTransferObject book)
        {
            Initialize((SaveBookInputDto)book);
        }

        private void Initialize(SaveBookInputDto book)
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
                foreach (var dto in book.Pages)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Book, Page>(RootEntity, () => new Page
                    {
                        Id = dto.PageId,
                        Index = dto.Index,
                        BookId = dto.BookId
                    }, "Pages"));
                }
            }
        }

    }
}