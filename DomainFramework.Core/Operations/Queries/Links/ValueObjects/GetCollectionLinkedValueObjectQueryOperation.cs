using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class GetCollectionLinkedValueObjectQueryOperation<TEntity, TValueObject, TRepositoryKey> : IQueryOperation
        where TEntity : IEntity
        where TValueObject : IValueObject
    {
        public List<TValueObject> LinkedValueObjects { get; private set; }

        public Func<IValueObjectQueryRepository, TEntity, IAuthenticatedUser, List<TValueObject>> GetLinkedValueObjects { get; set; }

        public Func<IValueObjectQueryRepository, TEntity, IAuthenticatedUser, Task<List<TValueObject>>> GetLinkedValueObjectsAsync { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedValueObjects = GetLinkedValueObjects(
                (IValueObjectQueryRepository)repositoryContext.GetQueryRepository(typeof(TRepositoryKey)), 
                (TEntity)entity, 
                user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            LinkedValueObjects = await GetLinkedValueObjectsAsync(
                (IValueObjectQueryRepository)repositoryContext.GetQueryRepository(typeof(TRepositoryKey)), 
                (TEntity)entity, 
                user);
        }
    }
}
