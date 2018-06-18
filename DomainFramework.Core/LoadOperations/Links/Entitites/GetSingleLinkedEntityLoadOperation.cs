using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetSingleLinkedEntityLoadOperation<TLinkedEntity> : ILoadOperation
        where TLinkedEntity : IEntity
    {
        public TLinkedEntity LinkedEntity { get; private set; }

        public Func<IQueryEntityRepository, IEntity, IAuthenticatedUser, TLinkedEntity> GetLinkedEntity { get; set; }

        public Func<IQueryEntityRepository, IEntity, IAuthenticatedUser, Task<TLinkedEntity>> GetLinkedEntityAsync { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedEntity = GetLinkedEntity(
                repositoryContext.GetQueryRepository(typeof(TLinkedEntity)), 
                entity, 
                user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedEntity = await GetLinkedEntityAsync(
                repositoryContext.GetQueryRepository(typeof(TLinkedEntity)), 
                entity, 
                user);
        }
    }
}
