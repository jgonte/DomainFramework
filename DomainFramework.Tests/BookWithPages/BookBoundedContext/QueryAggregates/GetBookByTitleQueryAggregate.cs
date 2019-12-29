using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class GetBookByTitleQueryAggregate : QueryAggregate<Book, BookOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<Page> GetPagesQueryOperation { get; }

        public IEnumerable<Page> Pages => GetPagesQueryOperation.LinkedEntities;

        public GetBookByTitleQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(BookWithPagesConnectionClass.GetConnectionName());

            BookQueryRepository.Register(context);

            PageQueryRepository.Register(context);

            RepositoryContext = context;

            GetPagesQueryOperation = new GetCollectionLinkedEntityQueryOperation<Page>
            {
                GetLinkedEntities = (repository, entity, user) => ((PageQueryRepository)repository).GetAllPagesForBook(RootEntity.Id).ToList(),
                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PageQueryRepository)repository).GetAllPagesForBookAsync(RootEntity.Id);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(GetPagesQueryOperation);
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

            PopulateDto(RootEntity);

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

            PopulateDto(RootEntity);

            return OutputDto;
        }

        public List<PageOutputDto> GetPagesDtos()
        {
            return Pages
                .Select(e => new PageOutputDto
                {
                    PageId = e.Id.Value,
                    Index = e.Index,
                    BookId = e.BookId
                })
                .ToList();
        }

        public override void PopulateDto(Book entity)
        {
            OutputDto.BookId = entity.Id.Value;

            OutputDto.Title = entity.Title;

            OutputDto.Category = entity.Category;

            OutputDto.DatePublished = entity.DatePublished;

            OutputDto.PublisherId = entity.PublisherId;

            OutputDto.IsHardCopy = entity.IsHardCopy;

            OutputDto.Pages = GetPagesDtos();
        }

    }
}