using System;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Core
{
    /// <summary>
    /// Operation to persist an entity inside a transaction
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to be persisted inside the transaction</typeparam>
    public class EntityCommandTransactedOperation<TEntity> : ITransactedOperation
        where TEntity : IEntity
    {
        private TEntity _entity;

        private CommandOperationTypes _operationType;

        public EntityCommandTransactedOperation(TEntity entity, CommandOperationTypes operationType)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entity = entity;

            _operationType = operationType;
        }

        public bool RequiresUnitOfWork => false; // By itself it does not require a transaction (one save)

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            switch (_operationType)
            {
                case CommandOperationTypes.Save:
                    {
                        repository.Save(_entity, user, unitOfWork);
                    }
                    break;
                case CommandOperationTypes.Create:
                    {
                        repository.Insert(_entity, user, unitOfWork);
                    }
                    break;
                case CommandOperationTypes.Update:
                    {
                        repository.Update(_entity, user, unitOfWork);
                    }
                    break;
                case CommandOperationTypes.Delete:
                    {
                        repository.Delete(_entity, user, unitOfWork);
                    }
                    break;
                default: throw new InvalidOperationException();
            }
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = (IEntityCommandRepository)repositoryContext.CreateCommandRepository(typeof(TEntity));

            switch (_operationType)
            {
                case CommandOperationTypes.Save:
                    {
                        await repository.SaveAsync(_entity, user, unitOfWork);
                    }
                    break;
                case CommandOperationTypes.Create:
                    {
                        await repository.InsertAsync(_entity, user, unitOfWork);
                    }
                    break;
                case CommandOperationTypes.Update:
                    {
                        await repository.UpdateAsync(_entity, user, unitOfWork);
                    }
                    break;
                case CommandOperationTypes.Delete:
                    {
                        await repository.DeleteAsync(_entity, user, unitOfWork);
                    }
                    break;
                default: throw new InvalidOperationException();
            }
        }
    }
}
