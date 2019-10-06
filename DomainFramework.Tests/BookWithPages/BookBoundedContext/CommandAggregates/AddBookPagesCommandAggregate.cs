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

        public AddBookPagesCommandAggregate(BookAddPagesInputDto book, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            Initialize(book, dependencies);
        }

        public override void Initialize(IInputDataTransferObject book, EntityDependency[] dependencies)
        {
            Initialize((BookAddPagesInputDto)book, dependencies);
        }

        private void Initialize(BookAddPagesInputDto book, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Page>(() => new PageCommandRepository());

            RootEntity = new Book
            {
                Id = book.Id
            };

            if (book.Pages?.Any() == true)
            {
                foreach (var page in book.Pages)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Book, Page>(RootEntity, () => new Page
                    {
                        Index = page.Index
                    }, "Pages"));
                }
            }
        }

    }
}