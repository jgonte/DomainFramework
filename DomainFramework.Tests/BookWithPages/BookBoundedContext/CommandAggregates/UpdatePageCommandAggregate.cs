using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookWithPages.BookBoundedContext
{
    public class UpdatePageCommandAggregate : CommandAggregate<Page>
    {
        public UpdatePageCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
        }

        public UpdatePageCommandAggregate(SavePageInputDto page, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            Initialize(page, dependencies);
        }

        public override void Initialize(IInputDataTransferObject page, EntityDependency[] dependencies)
        {
            Initialize((SavePageInputDto)page, dependencies);
        }

        private void Initialize(SavePageInputDto page, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Page>(() => new PageCommandRepository());

            var bookDependency = (Book)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Page
            {
                Id = page.PageId,
                Index = page.Index,
                BookId = (bookDependency != null) ? bookDependency.Id : page.BookId
            };

            Enqueue(new UpdateEntityCommandOperation<Page>(RootEntity, dependencies));
        }

    }
}