using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Operation to explicitly update an entity inside a transaction
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to be updated inside the transaction</typeparam>
    public class UpdateEntityTransactedOperation<TEntity> : ITransactedOperation
        where TEntity : IEntity
    {
        private TEntity _entity;

        public bool RequiresUnitOfWork => false; // By itself it does not require a transaction (one save)

        public UpdateEntityTransactedOperation(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entity = entity;
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            repository.Update(_entity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await repository.UpdateAsync(_entity, user, unitOfWork);
        }
    }
}
