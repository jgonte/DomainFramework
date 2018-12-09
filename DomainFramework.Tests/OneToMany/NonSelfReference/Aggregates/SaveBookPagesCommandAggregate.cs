using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DomainFramework.Tests
{
    class SaveBookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        private CollectionEntityLinkTransactedOperation<BookEntity, PageEntity> _pagesLink;

        public IEnumerable<PageEntity> Pages => _pagesLink.LinkedEntities;

        public SaveBookPagesCommandAggregate(RepositoryContext context, BookPagesInputDto bookPages) : base(context, null)
        {
            RootEntity = new BookEntity
            {
                Data = new BookData
                {
                    Title = bookPages.Title
                }
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<BookEntity>(RootEntity, CommandOperationTypes.Save)
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

                TransactedOperations.Enqueue(
                    _pagesLink
                );
            }           
        }
    }
}
