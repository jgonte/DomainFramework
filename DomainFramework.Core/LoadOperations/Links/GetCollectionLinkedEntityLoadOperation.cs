using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetCollectionLinkedEntityLoadOperation<TLinkedEntity> : ILoadOperation
        where TLinkedEntity : IEntity
    {
        public List<TLinkedEntity> LinkedEntities { get; private set; }

        public Func<IQueryRepository, IEntity, IAuthenticatedUser, List<TLinkedEntity>> GetLinkedEntities { get; set; }

        public Func<IQueryRepository, IEntity, IAuthenticatedUser, Task<List<TLinkedEntity>>> GetLinkedEntitiesAsync { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedEntities = GetLinkedEntities(
                repositoryContext.GetQueryRepository(typeof(TLinkedEntity)), 
                entity, 
                user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedEntities = await GetLinkedEntitiesAsync(
                repositoryContext.GetQueryRepository(typeof(TLinkedEntity)), 
                entity, 
                user);
        }
    }
}
