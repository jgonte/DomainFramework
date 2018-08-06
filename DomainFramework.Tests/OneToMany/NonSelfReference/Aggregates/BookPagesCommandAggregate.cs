using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class BookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        private CollectionEntityLinkTransactedOperation<BookEntity, PageEntity> _pagesLink;

        public IEnumerable<PageEntity> Pages => _pagesLink.LinkedEntities;

        public BookPagesCommandAggregate(RepositoryContext context, BookPagesDto bookPages) : base(context, null)
        {
            RootEntity = new BookEntity
            {
                Data = new BookData
                {
                    Title = bookPages.Title
                }
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<BookEntity>(RootEntity)
            );

            // Create it regardless to wheather the pages are provided so the zero linked entitities can be accesses if needed to
            _pagesLink = new CollectionEntityLinkTransactedOperation<BookEntity, PageEntity>(RootEntity);

            if (bookPages.Pages?.Any() == true)
            {
                foreach (var page in bookPages.Pages)
                {
                    var pageEntity = new PageEntity
                    {
                        Data = new PageData
                        {
                            Index = page.Index
                        }
                    };

                    _pagesLink.AddLinkedEntity(pageEntity);
                }

                TransactedSaveOperations.Enqueue(
                    _pagesLink
                );
            }           

            TransactedDeleteOperations.Enqueue(
                new DeleteEntityTransactedOperation<BookEntity>(RootEntity)
            );
        }
    }
}
