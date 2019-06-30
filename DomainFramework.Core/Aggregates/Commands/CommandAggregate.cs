using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CommandAggregate<TEntity>
        where TEntity : class, IEntity
    {
        private IRepositoryContext _repositoryContext;

        /// <summary>
        /// The save operations that are performed inside a unit of work
        /// </summary>
        private Queue<ICommandOperation> _commandOperations { get; set; } = new Queue<ICommandOperation>();

        public TEntity RootEntity { get; protected set; }

        public CommandAggregate()
        {
        }

        public CommandAggregate(IRepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void RegisterCommandRepositoryFactory<T>(Func<ICommandRepository> factory)
        {
            _repositoryContext.RegisterCommandRepositoryFactory<T>(factory);
        }

        public void Enqueue(ICommandOperation operation)
        {
            _commandOperations.Enqueue(operation);
        }

        public virtual void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && _commandOperations.Count() > 1)
            {
                unitOfWork = _repositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var operation in _commandOperations)
            {
                operation.Execute(_repositoryContext, user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public virtual async Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && _commandOperations.Count() > 1)
            {
                unitOfWork = _repositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new Queue<Task>();

            foreach (var operation in _commandOperations)
            {
                tasks.Enqueue(
                    operation.ExecuteAsync(_repositoryContext, user, unitOfWork)
                );
            }

            await Task.WhenAll(tasks);

            if (ownsUnitOfWork)
            {
                await unitOfWork.SaveAsync();
            }
        }

    }
}
