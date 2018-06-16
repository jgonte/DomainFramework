using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class SingleEntityLinkTransactedOperation<TEntity, TLinkedEntity> : ITransactedOperation
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        private IEntity _rootEntity;

        private TLinkedEntity _linkedEntity;

        public bool RequiresUnitOfWork => false; // By itself it does not require a transaction (single save)

        public SingleEntityLinkTransactedOperation(TEntity rootEntity, TLinkedEntity linkedEntity)
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
            var repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            repository.TransferEntities = () => new IEntity[] { _rootEntity };

            repository.Save(_linkedEntity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

            repository.TransferEntities = () => new IEntity[] { _rootEntity };

            await repository.SaveAsync(_linkedEntity, user, unitOfWork);
        }
    }
}
