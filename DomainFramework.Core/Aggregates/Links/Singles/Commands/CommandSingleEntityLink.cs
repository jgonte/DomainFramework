using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CommandSingleEntityLink<TEntity, TLinkedEntity> : ICommandSingleEntityLink<TEntity>,
        ISingleEntityLink<TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public Type LinkedEntityType => typeof(TLinkedEntity);

        public TLinkedEntity LinkedEntity { get; set; }

        public IEntity GetLinkedEntity() => LinkedEntity;

        public virtual void Save(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity)
        {
            if (LinkedEntity == null)
            {
                return;
            }

            var repository = repositoryContext.GetCommandRepository(LinkedEntityType);

            repository.TransferEntities = () => new IEntity[] { rootEntity };

            repository.Save(LinkedEntity, unitOfWork);
        }

        public async Task SaveAsync(IRepositoryContext repositoryContext, IUnitOfWork unitOfWork, TEntity rootEntity)
        {
            if (LinkedEntity == null)
            {
                return;
            }

            var repository = repositoryContext.GetCommandRepository(LinkedEntityType);

            repository.TransferEntities = () => new IEntity[] { rootEntity };

            await repository.SaveAsync(LinkedEntity, unitOfWork);
        }

        public void SetEntity(TLinkedEntity entity)
        {
            LinkedEntity = entity;
        }
    }
}
