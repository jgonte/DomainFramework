using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteBookPagesCommandAggregate : CommandAggregate<BookEntity>
    {
        public DeleteBookPagesCommandAggregate(RepositoryContext context, int? bookId) : base(context, null)
        {
            RootEntity = new BookEntity
            {
                Id = bookId
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<BookEntity>(RootEntity, CommandOperationTypes.Delete)
            );
        }
    }
}
