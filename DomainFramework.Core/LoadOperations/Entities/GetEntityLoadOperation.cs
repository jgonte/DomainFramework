using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetEntityLoadOperation<TEntity> : ILoadOperation
        where TEntity : IEntity
    {
        public TEntity Entity { get; private set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            Entity = (TEntity)repositoryContext.GetQueryRepository(typeof(TEntity))
                .GetById(entity.Id, user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            var e = await repositoryContext.GetQueryRepository(typeof(TEntity)).GetByIdAsync(entity.Id, user);

            Entity = (TEntity)e;
        }
    }
}
