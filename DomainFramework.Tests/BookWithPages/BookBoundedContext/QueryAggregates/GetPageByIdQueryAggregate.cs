using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class GetPageByIdQueryAggregate : GetByIdQueryAggregate<Page, int, PageOutputDto>
    {
        public GetPageByIdQueryAggregate() : this(null)
        {
        }

        public GetPageByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PageQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.PageId = RootEntity.Id;

            OutputDto.Index = RootEntity.Index;

            OutputDto.BookId = RootEntity.BookId;
        }

    }
}