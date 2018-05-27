﻿using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICollectionEntityLink : IEntityLink
    {
        IEnumerable<IEntity> GetLinkedEntities();
    }

    public interface ICollectionEntityLink<TEntity, TLinkedEntity> : ICollectionEntityLink, 
        IEntityLink<TEntity, TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity: IEntity
    {
        List<TLinkedEntity> LinkedEntities { get; set; }
    }
}