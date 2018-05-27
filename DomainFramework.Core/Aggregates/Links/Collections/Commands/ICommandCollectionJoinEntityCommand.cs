using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    public interface ICommandCollectionJoinEntityCommand<TJoinEntity> where TJoinEntity : IEntity
    {
        Type JoinEntityType { get; }

        List<TJoinEntity> JoinEntities { get; set; }

        void AddJoinEntity(TJoinEntity joinEntity);
    }
}