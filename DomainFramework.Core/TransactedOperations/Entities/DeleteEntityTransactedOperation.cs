using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Operation to delete an entity inside a transaction
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to be deleted inside the transaction</typeparam>
    public class DeleteEntityTransactedOperation<TEntity> : ITransactedOperation
        where TEntity : IEntity
    {
        private TEntity _entity;

        public bool RequiresUnitOfWork => false; // By itself it does not require a transaction (one delete)

        public DeleteEntityTransactedOperation(TEntity entity)
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

            repository.Delete(_entity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            await repository.DeleteAsync(_entity, user, unitOfWork);
        }
    }
}
