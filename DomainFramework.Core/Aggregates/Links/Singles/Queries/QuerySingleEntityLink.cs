using System;

namespace DomainFramework.Core
{
    public abstract class QuerySingleEntityLink<TEntity, TLinkedEntity> : IQuerySingleEntityLink<TEntity, TLinkedEntity>,
        ISingleEntityLink<TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public Type LinkedEntityType => typeof(TLinkedEntity);

        public TLinkedEntity LinkedEntity { get; set; }

        public IEntity GetLinkedEntity() => LinkedEntity;

        public void PopulateEntity(IQueryRepository repository, IEntity entity)
        {
            PopulateEntity(repository, (TEntity)entity);
        }

        public abstract void PopulateEntity(IQueryRepository repository, TEntity entity);
    }
}
