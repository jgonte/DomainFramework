using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class GetBookByTitleQueryAggregate : QueryAggregate<Book, BookOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int, Page, PageOutputDto> GetAllPagesLinkedAggregateQueryOperation { get; set; }

        public GetBookByTitleQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            BookQueryRepository.Register(context);

            PageQueryRepository.Register(context);

            GetAllPagesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int, Page, PageOutputDto>
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

        public BookOutputDto Get(string title)
        {
            var repository = (BookQueryRepository)RepositoryContext.GetQueryRepository(typeof(Book));

            RootEntity = repository.GetBookByTitle(title);

            if (RootEntity == null)
            {
                return null;
            }

            LoadLinks(null);

            PopulateDto();

            return OutputDto;
        }

        public async Task<BookOutputDto> GetAsync(string title)
        {
            var repository = (BookQueryRepository)RepositoryContext.GetQueryRepository(typeof(Book));

            RootEntity = await repository.GetBookByTitleAsync(title);

            if (RootEntity == null)
            {
                return null;
            }

            await LoadLinksAsync(null);

            PopulateDto();

            return OutputDto;
        }

        public override void PopulateDto()
        {
            OutputDto.BookId = RootEntity.Id;

            OutputDto.Title = RootEntity.Title;

            OutputDto.Category = RootEntity.Category;

            OutputDto.DatePublished = RootEntity.DatePublished;

            OutputDto.PublisherId = RootEntity.PublisherId;

            OutputDto.IsHardCopy = RootEntity.IsHardCopy;

            OutputDto.Pages = GetAllPagesLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}