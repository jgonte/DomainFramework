using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class BookPagesQueryAggregate : GetByIdQueryAggregate<BookEntity, int?, BookPagesOutputDto>
    {
        public GetCollectionLinkedEntityLoadOperation<PageEntity> GetPagesLoadOperation { get; }

        public IEnumerable<PageEntity> Pages => GetPagesLoadOperation.LinkedEntities;

        public BookPagesQueryAggregate() : this(null)
        {
        }

        public BookPagesQueryAggregate(RepositoryContext context) : base(context)
        {
            GetPagesLoadOperation = new GetCollectionLinkedEntityLoadOperation<PageEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((PageQueryRepository)repository).GetForBook(RootEntity.Id, user).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PageQueryRepository)repository).GetForBookAsync(RootEntity.Id, user: null);

                    return entities.ToList();
                }
            };

            LoadOperations.Enqueue(
                GetPagesLoadOperation
            );
        }

        public override BookPagesOutputDto GetDataTransferObject()
        {
            var dto = new BookPagesOutputDto();

            dto.Id = RootEntity.Id;

            dto.Title = RootEntity.Title;

            dto.Pages = Pages.Select(page => new PageOutputDto
            {
                Id = page.Id,
                Index = page.Index,
                BookId = page.BookId
            })
            .ToList();

            return dto;
        }
    }
}