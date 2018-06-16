using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Operation to persist a base entity inside a transaction
    /// </summary>
    /// <typeparam name="TBaseEntity">The type of entity to be persisted inside the transaction</typeparam>
    public class SaveBaseEntityTransactedOperation<TBaseEntity, TDerivedEntity> : IInheritanceTransactedOperation
        where TBaseEntity : IEntity
        where TDerivedEntity : IEntity
    {
        private TBaseEntity _baseEntity;

        /// <summary>
        /// The repository to be passed between the save base entity transacted operations
        /// </summary>
        public ICommandEntityRepository Repository { get; set; }

        public bool RequiresUnitOfWork => false; // By itself it does not require a transaction (one save)

        public SaveBaseEntityTransactedOperation(TBaseEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _baseEntity = entity;
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            // Use the existing repository or create one if it does not exist
            if (Repository == null)
            {
                Repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TBaseEntity));
            }

            Repository.Save(_baseEntity, user, unitOfWork);

            // Setup the derived repository for later use
            Repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TDerivedEntity));

            Repository.TransferEntities = () => new IEntity[] { _baseEntity };
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            // Use the existing repository or create one if it does not exist
            if (Repository == null)
            {
                Repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TBaseEntity));
            }

            await Repository.SaveAsync(_baseEntity, user, unitOfWork);

            // Setup the derived repository for later use
            Repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TDerivedEntity));

            Repository.TransferEntities = () => new IEntity[] { _baseEntity };
        }
    }
}
