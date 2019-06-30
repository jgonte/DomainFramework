using DomainFramework.Core;

namespace DomainFramework.Tests
{
    internal class ReplacePagesAggregate : CommandAggregate<BookEntity>
    {
        public ReplacePagesAggregate(RepositoryContext context, BookEntity bookEntity, PageInputDto[] pageInputDtos) : base(context)
        {
            RootEntity = bookEntity;

            var removeLinkedEntities = new BookDeletePagesCommandOperation(bookEntity);

            Enqueue(removeLinkedEntities);

            foreach (var pageInputDto in pageInputDtos)
            {
                var addPage = new AddLinkedEntityCommandOperation<BookEntity, PageEntity>(
                    bookEntity,
                    () => new PageEntity
                    {
                        Index = pageInputDto.Index
                    });

                Enqueue(addPage);
            }
        }
    }
}