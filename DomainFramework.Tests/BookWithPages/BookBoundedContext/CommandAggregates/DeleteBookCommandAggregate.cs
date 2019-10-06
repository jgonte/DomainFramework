using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookWithPages.BookBoundedContext
{
    public class DeleteBookCommandAggregate : CommandAggregate<Book>
    {
        public DeleteBookCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
        }

        public DeleteBookCommandAggregate(DeleteBookInputDto book, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            Initialize(book, dependencies);
        }

        public override void Initialize(IInputDataTransferObject book, EntityDependency[] dependencies)
        {
            Initialize((DeleteBookInputDto)book, dependencies);
        }

        private void Initialize(DeleteBookInputDto book, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Book>(() => new BookCommandRepository());

            RootEntity = new Book
            {
                Id = book.Id
            };

            Enqueue(new DeleteEntityCommandOperation<Book>(RootEntity));
        }

    }
}