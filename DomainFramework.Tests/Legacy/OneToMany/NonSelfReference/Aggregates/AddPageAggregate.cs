using DomainFramework.Core;

namespace DomainFramework.Tests
{
    internal class AddPageAggregate : CommandAggregate<BookEntity>
    {
        public AddPageAggregate(RepositoryContext context, BookEntity bookEntity, PageInputDto pageInputDto) : base(context)
        {
            RootEntity = bookEntity;

            var addPage = new AddLinkedEntityCommandOperation<BookEntity, PageEntity>(
                RootEntity,
                getLinkedEntity: () => new PageEntity
                {
                    Index = pageInputDto.Index
                }
            );

            Enqueue(addPage);
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}