using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    internal class BookDeletePagesCommandOperation : CommandOperation<BookEntity>
    {
        public BookDeletePagesCommandOperation(BookEntity entity) : base(entity)
        {
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(Entity.GetType());

            repository.DeleteCollection(Entity, user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(Entity.GetType());

            await repository.DeleteCollectionAsync(Entity, user, unitOfWork);
        }
    }
}