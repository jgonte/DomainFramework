using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    public class CommandCollectionBinaryEntityLink<TEntity, TLinkedEntity, TJoinEntity> : CommandCollectionEntityLink<TEntity, TLinkedEntity>,
        ICommandCollectionJoinEntityCommand<TJoinEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
        where TJoinEntity : IEntity
    {
        public Type JoinEntityType => typeof(TJoinEntity);

        public List<TJoinEntity> JoinEntities { get; set; } = new List<TJoinEntity>();

        public void AddJoinEntity(TJoinEntity joinEntity)
        {
            JoinEntities.Add(joinEntity);
        }

        public override void Save(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity)
        {
            var linkedEntityRepository = repositoryContext.GetCommandRepository(LinkedEntityType);

            var joinEntityRepository = repositoryContext.GetCommandRepository(JoinEntityType);

            var i = 0;

            foreach (var linkedEntity in LinkedEntities)
            {
                linkedEntityRepository.TransferEntities = () =>new IEntity[] { rootEntity };

                linkedEntityRepository.Save(linkedEntity, unitOfWork);

                joinEntityRepository.TransferEntities = () => new IEntity[] { rootEntity, linkedEntity };

                joinEntityRepository.Save(JoinEntities[i++], unitOfWork);
            }
        }
    }
}