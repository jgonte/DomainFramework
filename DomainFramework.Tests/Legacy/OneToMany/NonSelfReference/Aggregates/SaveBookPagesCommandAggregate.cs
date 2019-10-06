using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DomainFramework.Tests
{
    class SaveBookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        public List<PageEntity> Pages { get; set; } = new List<PageEntity>();

        public SaveBookPagesCommandAggregate(RepositoryContext context, BookPagesInputDto bookPages) : base(context)
        {
            RootEntity = new BookEntity
            {
                Id = bookPages.Id,
                Title = bookPages.Title
            };

            Enqueue(
                new SaveEntityCommandOperation<BookEntity>(RootEntity)
            );

            if (bookPages.Pages?.Any() == true)
            {
                foreach (var page in bookPages.Pages)
                {
                    var pageEntity = new PageEntity
                    {
                        Id = page.Id,
                        Index = page.Index
                    };

                    Pages.Add(pageEntity);

                    if (pageEntity.Id != null)
                    {
                        pageEntity.BookId = RootEntity.Id.Value;

                        Enqueue(
                            new UpdateEntityCommandOperation<PageEntity>(pageEntity)
                        );
                    }
                    else
                    {
                        Enqueue(
                            new AddLinkedEntityCommandOperation<BookEntity, PageEntity>(
                                RootEntity,
                                getLinkedEntity: () => pageEntity
                            )
                        );
                    }
                }
            }           
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}
