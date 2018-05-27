using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICollectionEntityLink : IEntityLink
    {
        /// <summary>
        /// Retrieves the entities to persist or the ones read
        /// </summary>
        /// <returns></returns>
        IEnumerable<IEntity> GetLinkedEntities();
    }

    public interface ICollectionEntityLink<TLinkedEntity> : ICollectionEntityLink
    {
        List<TLinkedEntity> LinkedEntities { get; set; }
    }
}
