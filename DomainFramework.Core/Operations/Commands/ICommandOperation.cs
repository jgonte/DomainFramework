using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public interface ICommandOperation
    {
        void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork);

        Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork);
    }
}