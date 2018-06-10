using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class BookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        private CollectionEntityLinkTransactedOperation<BookEntity, PageEntity> _pagesLink;

        public IEnumerable<PageEntity> Pages => _pagesLink.LinkedEntities;

        public BookPagesCommandAggregate(RepositoryContext context, BookEntity entity) : base(context, entity)
        {
            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<BookEntity>(entity)
            );

            _pagesLink = new CollectionEntityLinkTransactedOperation<BookEntity, PageEntity>(entity);

            TransactedSaveOperations.Enqueue(
                _pagesLink
            );

            TransactedDeleteOperations.Enqueue(
                new DeleteEntityTransactedOperation<BookEntity>(entity)
            );
        }

        public void AddPage(PageEntity pageEntity)
        {
            _pagesLink.AddLinkedEntity(pageEntity);
        }
    }
}
