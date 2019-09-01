using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface IQueryOperation
    {
        /// <summary>
        /// Executes the operation
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        void Execute(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user);

        /// <summary>
        /// Executes the operation asynchronously
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task ExecuteAsync(IRepositoryContext repositoryContext, IEntity entity, IAuthenticatedUser user);
    }
}