using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Operation to persist an entity with an inheritance hierarchy inside a transaction
    /// </summary>
    /// <typeparam name="TEntity">The type of entity to be persisted inside the transaction</typeparam>
    public class SaveEntityWithInheritanceTransactedOperation<TEntity> : ITransactedOperation
        where TEntity : IEntity
    {
        private TEntity _entity;

        public Queue<IInheritanceTransactedOperation> InheritanceTransactedOperations { get; set; } = new Queue<IInheritanceTransactedOperation>();

        public bool RequiresUnitOfWork => true; // Always more than one save

        public SaveEntityWithInheritanceTransactedOperation(TEntity entity, params IInheritanceTransactedOperation[] inheritanceHierarchy)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entity = entity;

            if (inheritanceHierarchy != null)
            {
                foreach (var operation in inheritanceHierarchy)
                {
                    InheritanceTransactedOperations.Enqueue(operation);
                }
            }
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            ICommandRepository repository = null;

            foreach (var operation in InheritanceTransactedOperations)
            {
                operation.Repository = repository; // Set existing repository

                operation.Execute(repositoryContext, user, unitOfWork);

                repository = operation.Repository; // Get new repository
            }

            repository.Save(_entity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            ICommandRepository repository = null;

            var tasks = new Queue<Task>();

            foreach (var operation in InheritanceTransactedOperations)
            {
                operation.Repository = repository; // Set existing repository

                tasks.Enqueue(
                    operation.ExecuteAsync(repositoryContext, user, unitOfWork)
                );

                repository = operation.Repository; // Get new repository
            }

            tasks.Enqueue(
                repository.SaveAsync(_entity, user, unitOfWork)
            );

            await Task.WhenAll(tasks);
        }
    }
}
