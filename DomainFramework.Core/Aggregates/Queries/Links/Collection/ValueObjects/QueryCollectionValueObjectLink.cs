using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryCollectionValueObjectLink<TEntity, TLinkedValueObject> : IQueryCollectionValueObjectLink
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        public List<TLinkedValueObject> LinkedValueObjects { get; set; }

        IEnumerable<IValueObject> IQueryCollectionValueObjectLink.LinkedValueObjects => LinkedValueObjects.Cast<IValueObject>();

        public void PopulateValueObjects(IRepositoryContext repositoryContext, IEntity entity)
        {
            PopulateValueObjects(repositoryContext, (TEntity)entity);
        }

        public abstract void PopulateValueObjects(IRepositoryContext repositoryContext, TEntity entity);

        public async Task PopulateValueObjectsAsync(IRepositoryContext repositoryContext, IEntity entity)
        {
            await PopulateValueObjectsAsync(repositoryContext, (TEntity)entity);
        }

        public abstract Task PopulateValueObjectsAsync(IRepositoryContext repositoryContext, TEntity entity);
    }
}
