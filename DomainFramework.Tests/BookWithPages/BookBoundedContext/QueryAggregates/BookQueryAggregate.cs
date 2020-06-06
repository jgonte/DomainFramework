using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class BookQueryAggregate : GetByIdQueryAggregate<Book, int?, BookOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Page, PageOutputDto> GetAllPagesLinkedAggregateQueryOperation { get; set; }

        public BookQueryAggregate() : this(null)
        {
        }

        public BookQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            BookQueryRepository.Register(context);

            PageQueryRepository.Register(context);

            GetAllPagesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Page, PageOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PageQueryRepository)repository).GetAllPagesForBook(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PageQueryRepository)repository).GetAllPagesForBookAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Page)
                    {
                        return new GetPageByIdQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllPagesLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.BookId = RootEntity.Id.Value;

            OutputDto.Title = RootEntity.Title;

            OutputDto.Category = RootEntity.Category;

            OutputDto.DatePublished = RootEntity.DatePublished;

            OutputDto.PublisherId = RootEntity.PublisherId;

            OutputDto.IsHardCopy = RootEntity.IsHardCopy;

            OutputDto.Pages = GetAllPagesLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}