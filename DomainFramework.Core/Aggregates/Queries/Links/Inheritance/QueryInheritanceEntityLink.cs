using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryInheritanceEntityLink<TKey, TBaseEntity> : IQueryInheritanceEntityLink<TKey>
        where TBaseEntity : IEntity
    {
        public TBaseEntity BaseEntity { get; set; }

        IEntity IQueryInheritanceEntityLink<TKey>.BaseEntity { get => BaseEntity; }

        public abstract void PopulateEntity(IRepositoryContext repositoryContext, TKey derivedEntityId);

        public abstract Task PopulateEntityAsync(IRepositoryContext repositoryContext, TKey derivedEntityId);
    }
}
