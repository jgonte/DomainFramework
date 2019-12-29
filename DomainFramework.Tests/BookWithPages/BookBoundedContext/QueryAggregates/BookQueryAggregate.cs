using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookWithPages.BookBoundedContext
{
    public class BookQueryAggregate : GetByIdQueryAggregate<Book, int?, BookOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<Page> GetPagesQueryOperation { get; }

        public IEnumerable<Page> Pages => GetPagesQueryOperation.LinkedEntities;

        public BookQueryAggregate()
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