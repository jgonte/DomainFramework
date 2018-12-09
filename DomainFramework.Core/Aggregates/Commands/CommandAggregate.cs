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
        public Queue<ITransactedOperation> TransactedOperations { get; set; } = new Queue<ITransactedOperation>();

        protected virtual bool RequiresUnitOfWork => TransactedOperations.Count() > 1 || TransactedOperations.Any(to => to.RequiresUnitOfWork);

        public CommandAggregate(RepositoryContext context, TEntity entity = null)
        {
            RepositoryContext = context;

            RootEntity = entity;
        }

        public virtual void Save(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            foreach (var operation in TransactedOperations)
            {
                operation.Execute(RepositoryContext, user, unitOfWork);
            }

            if (ownsUnitOfWork)
            {
                unitOfWork.Save();
            }
        }

        public virtual async Task SaveAsync(IAuthenticatedUser user = null, IUnitOfWork unitOfWork = null)
        {
            var ownsUnitOfWork = false;

            if (unitOfWork == null && RequiresUnitOfWork)
            {
                unitOfWork = RepositoryContext.CreateUnitOfWork();

                ownsUnitOfWork = true;
            }

            var tasks = new Queue<Task>();

            foreach (var operation in TransactedOperations)
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
