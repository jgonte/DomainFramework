using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class SingleSymetricEntityLinkTransactedOperation<TEntity> : ITransactedOperation
        where TEntity : IEntity
    {
        private TEntity _rootEntity;

        private TEntity _linkedEntity;

        public bool RequiresUnitOfWork => true; // One save and one update

        public SingleSymetricEntityLinkTransactedOperation(TEntity rootEntity, TEntity linkedEntity)
        {
            if (rootEntity == null)
            {
                throw new ArgumentNullException(nameof(rootEntity));
            }

            if (linkedEntity == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity));
            }

            _rootEntity = rootEntity;

            _linkedEntity = linkedEntity;
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            linkedEntityRepository.TransferEntities = () => new IEntity[] { _rootEntity };

            linkedEntityRepository.Save(_linkedEntity, user, unitOfWork);

            // Add a command to update the root entity with the id of the linked one (symmetric relationship)
            var rootEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            rootEntityRepository.Update(_rootEntity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var linkedEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            linkedEntityRepository.TransferEntities = () => new IEntity[] { _rootEntity };

            await linkedEntityRepository.SaveAsync(_linkedEntity, user, unitOfWork);

            // Add a command to update the root entity with the id of the linked one (symmetric relationship)
            var rootEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await rootEntityRepository.UpdateAsync(_rootEntity, user, unitOfWork);
        }
    }
}
