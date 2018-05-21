using System;

namespace DomainFramework.Core
{
    public interface IEntityLink
    {
        /// <summary>
        /// The type of the linked entity to retrieve its repository
        /// </summary>
        Type LinkedEntityType { get; }

        /// <summary>
        /// Sets the value of the foreign key from the primary key of the primary entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="linkedEntity"></param>
        void SetForeignKey(IEntity entity, IEntity linkedEntity);
    }

    public interface IEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity: IEntity
    {
        /// <summary>
        /// Sets the value of the foreign key from the primary key of the primary entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="linkedEntity"></param>
        void SetForeignKey(TEntity entity, TLinkedEntity linkedEntity);
    }
}