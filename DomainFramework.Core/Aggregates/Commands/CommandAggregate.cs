using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Ensure business invariant by executing operations inside a unit fo work
    /// </summary>
    public class CommandAggregate<TEntity> : ICommandAggregate<TEntity>
        where TEntity : class, IEntity
    {
        public IRepositoryContext RepositoryContext { get; set; }

        public TEntity RootEntity { get; set; }

        /// <summary>
        /// The save operations that are performed inside a unit of work
        /// </summary>
        public Queue<ITransactedOperation> TransactedSaveOperations { get; set; } = new Queue<ITransactedOperation>();

        protected virtual bool RequiresSaveUnitOfWork => TransactedSaveOperations.Count() > 1 || TransactedSaveOperations.Any(to => to.RequiresUnitOfWork);

        /// <summary>
        /// The delete operations that are performed inside a unit of work
        /// </summary>
        public Queue<ITransactedOperation> TransactedDeleteOperations { get; set; } = new Queue<ITransactedOperation>();

        protected virtual bool RequiresDeleteUnitOfWork => TransactedDeleteOperations.Count() > 1 || TransactedDeleteOperations.Any(to => to.RequiresUnitOfWork);

        public CommandAggregate(RepositoryContext context, TEntity entity = null)
        {
            RepositoryContext = context;

            RootEntity = entity;
        }

        public virtual void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null, IEnumerable<IEntity> transferEntities = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresSaveUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var operation in TransactedSaveOperations)
            {
                operation.Execute(RepositoryContext, user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public virtual void Delete(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresDeleteUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var operation in TransactedDeleteOperations)
            {
                operation.Execute(RepositoryContext, user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public virtual async Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null, IEnumerable<IEntity> transferEntities = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresSaveUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new Queue<Task>();

            foreach (var operation in TransactedSaveOperations)
            {
                tasks.Enqueue(
                    operation.ExecuteAsync(RepositoryContext, user, unitOfWork)
                );
            }

            await Task.WhenAll(tasks);

            if (ownsUnitOfWork)
            {
                await unitOfWork.SaveAsync();
            }
        }

        public virtual async Task DeleteAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresDeleteUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new Queue<Task>();

            foreach (var operation in TransactedDeleteOperations)
            {
                tasks.Enqueue(
                    operation.ExecuteAsync(RepositoryContext, user, unitOfWork)
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
