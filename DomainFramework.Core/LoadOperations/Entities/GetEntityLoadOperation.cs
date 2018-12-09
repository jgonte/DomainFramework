using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetEntityLoadOperation<TEntity> : ILoadOperation
        where TEntity : IEntity
    {
        public TEntity Entity { get; private set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var repository = (IEntityQueryRepository)repositoryContext.GetQueryRepository(typeof(TEntity));

            Entity = (TEntity)repository.GetById(entity.Id, user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var repository = (IEntityQueryRepository)repositoryContext.GetQueryRepository(typeof(TEntity));

            Entity = (TEntity)await repository.GetByIdAsync(entity.Id, user);
        }
    }
}
