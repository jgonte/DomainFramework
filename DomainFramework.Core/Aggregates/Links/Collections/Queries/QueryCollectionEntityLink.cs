using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Core
{
    public abstract class QueryCollectionEntityLink<TEntity, TLinkedEntity> : IQueryCollectionEntityLink,
        ICollectionEntityLink<TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public Type LinkedEntityType => typeof(TLinkedEntity);

        public List<TLinkedEntity> LinkedEntities { get; set; }

        public IEnumerable<IEntity> GetLinkedEntities() => LinkedEntities.Cast<IEntity>();

        public void PopulateEntities(IQueryRepository repository, IEntity entity)
        {
            PopulateEntities(repository, (TEntity)entity);
        }

        public abstract void PopulateEntities(IQueryRepository repository, TEntity entity);
    }
}
