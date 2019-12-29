using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class BookPagesQueryAggregate : GetByIdQueryAggregate<BookEntity, int?, BookPagesOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<PageEntity> GetPagesLoadOperation { get; }

        public IEnumerable<PageEntity> Pages => GetPagesLoadOperation.LinkedEntities;

        public BookPagesQueryAggregate() : this(null)
        {
        }

        public BookPagesQueryAggregate(RepositoryContext context) : base(context)
        {
            GetPagesLoadOperation = new GetCollectionLinkedEntityQueryOperation<PageEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((PageQueryRepository)repository).GetForBook(RootEntity.Id).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PageQueryRepository)repository).GetForBookAsync(RootEntity.Id);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(
                GetPagesLoadOperation
            );
        }

        public override void PopulateDto(BookEntity entity)
        {
            OutputDto.Id = RootEntity.Id;

            OutputDto.Title = RootEntity.Title;

            OutputDto.Pages = Pages.Select(page => new PageOutputDto
            {
                Id = page.Id,
                Index = page.Index,
                BookId = page.BookId
            })
            .ToList();
        }
    }
}