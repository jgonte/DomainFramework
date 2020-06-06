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
            RegisterCommandRepositoryFactory<Book>(() => new BookCommandRepository());

            RootEntity = new Book
            {
                Id = book.BookId
            };

            if (book.Pages?.Any() == true)
            {
                foreach (var dto in book.Pages)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is SavePageInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Book, SavePageCommandAggregate, SavePageInputDto>(
                            RootEntity,
                            (SavePageInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Pages"
                                }
                            }
                        );

                        Enqueue(operation);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

    }
}