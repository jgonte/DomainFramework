using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CommandSingleSymmetricEntityLink<TEntity, TLinkedEntity> : ICommandSingleEntityLink<TEntity>,
        ISingleEntityLink<TLinkedEntity>
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        public TLinkedEntity LinkedEntity { get; set; }

        public IEntity GetLinkedEntity() => LinkedEntity;

        public virtual void Save(IRepositoryContext repositoryContext, TEntity rootEntity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            if (LinkedEntity == null)
            {
                return;
            }

            var linkedEntityRepository = repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            linkedEntityRepository.TransferEntities = () => new IEntity[] { rootEntity };

            linkedEntityRepository.Save(LinkedEntity, user, unitOfWork);

            // Add a command to update the root entity with the id of the linked one (symmetric relationship)
            var rootEntityRepository = repositoryContext.CreateCommandRepository(typeof(TEntity));

            rootEntityRepository.Update(rootEntity, user, unitOfWork);
        }

        public async Task SaveAsync(IRepositoryContext repositoryContext, TEntity rootEntity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            if (LinkedEntity == null)
            {
                return;
            }

            var linkedEntityRepository = repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            linkedEntityRepository.TransferEntities = () => new IEntity[] { rootEntity };

            await linkedEntityRepository.SaveAsync(LinkedEntity, user, unitOfWork);

            // Add a command to update the root entity with the id of the linked one (symmetric relationship)
            var rootEntityRepository = repositoryContext.CreateCommandRepository(typeof(TEntity));

            await rootEntityRepository.UpdateAsync(rootEntity, user, unitOfWork);
        }

        public void SetEntity(TLinkedEntity entity)
        {
            LinkedEntity = entity;
        }
    }
}
