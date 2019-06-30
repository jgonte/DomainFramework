using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class DeleteEntityCommandOperation<TEntity> : CommandOperation<TEntity>
        where TEntity : IEntity
    {
        public DeleteEntityCommandOperation(TEntity entity) : base(entity)
        {
        }

        public override void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            repository.Delete(Entity, user, unitOfWork);
        }

        public override async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await repository.DeleteAsync(Entity, user, unitOfWork);
        }
    }
}
