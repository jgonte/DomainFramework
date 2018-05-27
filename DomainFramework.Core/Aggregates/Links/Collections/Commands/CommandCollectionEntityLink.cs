using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CommandCollectionEntityLink<TEntity, TLinkedEntity> : ICommandCollectionEntityLink<TEntity, TLinkedEntity>,
        ICollectionEntityLink<TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public List<TLinkedEntity> LinkedEntities { get; set; } = new List<TLinkedEntity>();

        public Type LinkedEntityType => typeof(TLinkedEntity);

        public IEnumerable<IEntity> GetLinkedEntities() => LinkedEntities.Cast<IEntity>();

        public void AddEntity(TLinkedEntity entity)
        {
            LinkedEntities.Add(entity);
        }

        public virtual void Save(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity)
        {
            var repository = repositoryContext.GetCommandRepository(LinkedEntityType);

            foreach (var linkedEntity in LinkedEntities)
            {
                repository.TransferEntities = () =>new IEntity[] { rootEntity };

                repository.Save(linkedEntity, unitOfWork);
            }
        }

        public virtual async Task SaveAsync(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity)
        {
            var repository = repositoryContext.GetCommandRepository(LinkedEntityType);

            foreach (var linkedEntity in LinkedEntities)
            {
                repository.TransferEntities = () => new IEntity[] { rootEntity };

                await repository.SaveAsync(linkedEntity, unitOfWork);
            }
        }
    }
}
