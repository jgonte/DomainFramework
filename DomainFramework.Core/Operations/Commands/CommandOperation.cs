using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public abstract class CommandOperation<TEntity> : ICommandOperation
        where TEntity : IEntity
    {
        public TEntity Entity { get; private set; }

        public CommandOperation(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entity = entity;
        }

        /// <summary>
        /// Executes the operation
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="user"></param>
        /// <param name="unitOfWork"></param>
        public abstract void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        /// <summary>
        /// Executes the operation asynchronously
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="user"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        public abstract Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork);
    }
}
