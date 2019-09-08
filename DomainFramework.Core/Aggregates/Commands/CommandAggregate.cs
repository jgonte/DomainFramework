using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class CommandAggregate<TEntity> : ICommandAggregate<TEntity>
        where TEntity : class, IEntity
    {
        IRepositoryContext IAggregate.RepositoryContext { get; set; }

        TEntity IAggregate<TEntity>.RootEntity { get; set; }

        public TEntity RootEntity
        {
            get
            {
                return ((IAggregate<TEntity>)this).RootEntity;
            }

            protected set
            {
                ((IAggregate<TEntity>)this).RootEntity = value;
            }
        }

        /// <summary>
        /// The save operations that are performed inside a unit of work
        /// </summary>
        private Queue<ICommandOperation> _commandOperations { get; set; } = new Queue<ICommandOperation>();

        public CommandAggregate()
        {
        }

        public abstract void Initialize(IInputDataTransferObject inputDto);

        public CommandAggregate(IRepositoryContext repositoryContext)
        {
            ((IAggregate)this).RepositoryContext = repositoryContext;
        }

        public void RegisterCommandRepositoryFactory<T>(Func<ICommandRepository> factory)
        {
            ((IAggregate)this).RepositoryContext.RegisterCommandRepositoryFactory<T>(factory);
        }

        public void Enqueue(ICommandOperation operation)
        {
            _commandOperations.Enqueue(operation);
        }

        public virtual void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            var repositoryContext = ((IAggregate)this).RepositoryContext;

            if (unitOfWork == null && _commandOperations.Count() > 1)
            {
                unitOfWork = repositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var operation in _commandOperations)
            {
                operation.Execute(repositoryContext, user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public virtual async Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            var repositoryContext = ((IAggregate)this).RepositoryContext;

            if (unitOfWork == null && _commandOperations.Count() > 1)
            {
                unitOfWork = repositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new Queue<Task>();

            foreach (var operation in _commandOperations)
            {
                tasks.Enqueue(
                    operation.ExecuteAsync(repositoryContext, user, unitOfWork)
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
