using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetSingleLinkedEntityQueryOperation<TLinkedEntity> : IQueryOperation
        where TLinkedEntity : IEntity
    {
        public TLinkedEntity LinkedEntity { get; private set; }

        public Func<IEntityQueryRepository, IEntity, IAuthenticatedUser, TLinkedEntity> GetLinkedEntity { get; set; }

        public Func<IEntityQueryRepository, IEntity, IAuthenticatedUser, Task<TLinkedEntity>> GetLinkedEntityAsync { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedEntity = GetLinkedEntity(
                (IEntityQueryRepository)repositoryContext.GetQueryRepository(typeof(TLinkedEntity)), 
                entity, 
                user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedEntity = await GetLinkedEntityAsync(
                (IEntityQueryRepository)repositoryContext.GetQueryRepository(typeof(TLinkedEntity)), 
                entity, 
                user);
        }
    }
}
