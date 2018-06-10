using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICollectionEntityLink
    {
        IEnumerable<IEntity> GetLinkedEntities();
    }

    public interface ICollectionEntityLink<TEntity, TLinkedEntity> : ICollectionEntityLink
        where TEntity : IEntity
        where TLinkedEntity: IEntity
    {
        List<TLinkedEntity> LinkedEntities { get; set; }
    }
}