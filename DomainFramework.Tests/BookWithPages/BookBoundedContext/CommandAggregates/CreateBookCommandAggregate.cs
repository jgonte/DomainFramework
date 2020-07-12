using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookWithPages.BookBoundedContext
{
    public class CreateBookCommandAggregate : CommandAggregate<Book>
    {
        public CreateBookCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
        }

        public CreateBookCommandAggregate(SaveBookInputDto book, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
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

            RootEntity = new Book
            {
                Id = book.BookId,
                Title = book.Title,
                Category = book.Category,
                DatePublished = book.DatePublished,
                PublisherId = book.PublisherId,
                IsHardCopy = book.IsHardCopy
            };

            Enqueue(new InsertEntityCommandOperation<Book>(RootEntity, dependencies));

            if (book.Pages?.Any() == true)
            {
                foreach (var dto in book.Pages)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is SavePageInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Book, CreatePageCommandAggregate, SavePageInputDto>(
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