using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Core
{
    public abstract class CollectionEntityLink<TEntity, TLinkedEntity> : ICollectionEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public IEnumerable<IEntity> GetLinkedEntities() => LinkedEntities.Cast<IEntity>();

        public List<TLinkedEntity> LinkedEntities { get; set; } = new List<TLinkedEntity>();

        public Type LinkedEntityType => typeof(TLinkedEntity);

        public void AddEntity(TLinkedEntity entity)
        {
            LinkedEntities.Add(entity);
        }

        public void SetForeignKey(IEntity entity, IEntity linkedEntity)
        {
            SetForeignKey((TEntity)entity, (TLinkedEntity)linkedEntity);
        }

        public abstract void SetForeignKey(TEntity entity, TLinkedEntity linkedEntity);
    }
}