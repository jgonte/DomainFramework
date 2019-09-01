using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class SetCollectionLinkedValueObjectQueryOperation<TEntity, TRepositoryKey> : IQueryOperation
        where TEntity : IEntity
    {
        public Action<IValueObjectQueryRepository, TEntity, IAuthenticatedUser> SetLinkedValueObjects { get; set; }

        public Func<IValueObjectQueryRepository, TEntity, IAuthenticatedUser, Task> SetLinkedValueObjectsAsync { get; set; }

        public void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            SetLinkedValueObjects(
                (IValueObjectQueryRepository)repositoryContext.GetQueryRepository(typeof(TRepositoryKey)), 
                (TEntity)entity, 
                user);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user)
        {
            await SetLinkedValueObjectsAsync(
                (IValueObjectQueryRepository)repositoryContext.GetQueryRepository(typeof(TRepositoryKey)), 
                (TEntity)entity, 
                user);
        }
    }
}
