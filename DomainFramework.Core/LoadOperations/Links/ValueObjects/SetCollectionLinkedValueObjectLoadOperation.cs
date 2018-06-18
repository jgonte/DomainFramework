using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class SetCollectionLinkedValueObjectLoadOperation<TEntity, TLinkedValueObject> : ILoadOperation
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        public Action<IQueryEntityRepository, TEntity, IAuthenticatedUser> SetLinkedValueObjects { get; set; }

        public Func<IQueryEntityRepository, TEntity, IAuthenticatedUser, Task> SetLinkedValueObjectsAsync { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            SetLinkedValueObjects(
                repositoryContext.GetQueryRepository(typeof(TEntity)), 
                (TEntity)entity, 
                user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            await SetLinkedValueObjectsAsync(
                repositoryContext.GetQueryRepository(typeof(TEntity)), 
                (TEntity)entity, 
                user);
        }
    }
}
