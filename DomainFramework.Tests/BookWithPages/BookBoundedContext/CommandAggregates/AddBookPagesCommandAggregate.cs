using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookWithPages.BookBoundedContext
{
    public class AddBookPagesCommandAggregate : CommandAggregate<Book>
    {
        public AddBookPagesCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
        }

        public AddBookPagesCommandAggregate(BookAddPagesInputDto book) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            Initialize(book);
        }

        public override void Initialize(IInputDataTransferObject book)
        {
            Initialize((BookAddPagesInputDto)book);
        }

        private void Initialize(BookAddPagesInputDto book)
        {
            RegisterCommandRepositoryFactory<Page>(() => new PageCommandRepository());

            RootEntity = new Book
            {
                Id = book.Id
            };

            if (book.Pages?.Any() == true)
            {
                foreach (var dto in book.Pages)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Book, Page>(RootEntity, () => new Page
                    {
                        Index = dto.Index,
                        BookId = dto.BookId
                    }, "Pages"));
                }
            }
        }

    }
}