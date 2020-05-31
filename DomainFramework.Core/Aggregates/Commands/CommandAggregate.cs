using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class CommandAggregate<TEntity> : ICommandAggregate
        where TEntity : class, IEntity
    {
        public IRepositoryContext RepositoryContext { get; set; }

        IEntity IAggregate.RootEntity { get; set; }

        public TEntity RootEntity
        {
            get
            {
                return (TEntity)((IAggregate)this).RootEntity;
            }

            set
            {
                ((IAggregate)this).RootEntity = value;
            }
        }

        /// <summary>
        /// The save operations that are performed inside a unit of work
        /// </summary>
        private Queue<ICommandOperation> _commandOperations { get; set; } = new Queue<ICommandOperation>();
 
        public CommandAggregate()
        {
        }

        public abstract void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies);

        public CommandAggregate(IRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }

        public void RegisterCommandRepositoryFactory<T>(Func<ICommandRepository> factory)
        {
            RepositoryContext.RegisterCommandRepositoryFactory<T>(factory);
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
