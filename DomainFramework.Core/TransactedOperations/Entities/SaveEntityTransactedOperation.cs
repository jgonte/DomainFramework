using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Operation to persist an entity inside a transaction
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to be persisted inside the transaction</typeparam>
    public class SaveEntityTransactedOperation<TEntity> : ITransactedOperation
        where TEntity : IEntity
    {
        private TEntity _entity;

        public bool RequiresUnitOfWork => false; // By itself it does not require a transaction (one save)

        public SaveEntityTransactedOperation(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entity = entity;
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            repository.Save(_entity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await repository.SaveAsync(_entity, user, unitOfWork);
        }
    }
}
