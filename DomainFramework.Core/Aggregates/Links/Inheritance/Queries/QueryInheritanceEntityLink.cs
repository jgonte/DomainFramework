using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class QueryInheritanceEntityLink<TKey, TBaseEntity> : IQueryInheritanceEntityLink<TKey>,
        ISingleEntityLink<TBaseEntity>
        where TBaseEntity : IEntity
    {
        public Type LinkedEntityType => typeof(TBaseEntity);

        public TBaseEntity LinkedEntity { get; set; }

        public IEntity GetLinkedEntity() => LinkedEntity;

        public abstract void PopulateEntity(IRepositoryContext repositoryContext, TKey derivedEntityId);

        public abstract Task PopulateEntityAsync(IRepositoryContext repositoryContext, TKey derivedEntityId);
    }
}
