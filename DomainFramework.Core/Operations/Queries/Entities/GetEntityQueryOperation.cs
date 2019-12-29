using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetEntityQueryOperation<TEntity> : IQueryOperation
        where TEntity : IEntity
    {
        public TEntity Entity { get; private set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var repository = (IEntityQueryRepository)repositoryContext.GetQueryRepository(typeof(TEntity));

            Entity = (TEntity)repository.GetById(entity.Id);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var repository = (IEntityQueryRepository)repositoryContext.GetQueryRepository(typeof(TEntity));

            Entity = (TEntity)await repository.GetByIdAsync(entity.Id);
        }
    }
}
