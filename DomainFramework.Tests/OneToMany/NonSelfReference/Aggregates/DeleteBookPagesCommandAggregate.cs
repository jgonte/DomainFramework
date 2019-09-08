using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class DeleteBookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        public DeleteBookPagesCommandAggregate(RepositoryContext context, int? bookId) : base(context)
        {
            RootEntity = new BookEntity
            {
                Id = bookId
            };

            Enqueue(
                new DeleteEntityCommandOperation<BookEntity>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
