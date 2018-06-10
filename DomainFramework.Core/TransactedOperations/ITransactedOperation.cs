using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Represents an operation that is performed inside a transaction
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ITransactedOperation
    {
        /// <summary>
        /// Whether the transacted operation requires a unit of work
        /// </summary>
        // The rule is that each transacted operation reports if is is set up to have more than one save operation
        bool RequiresUnitOfWork { get; }

        /// <summary>
        /// Executes the operation
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="user"></param>
        /// <param name="unitOfWork"></param>
        void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        /// <summary>
        /// Executes the operation asynchronously
        /// </summary>
        /// <param name="repositoryContext"></param>
        /// <param name="user"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork);
    }
}