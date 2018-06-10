using System.Threading.Tasks;

namespace DomainFramework.Core
{
    /// <summary>
    /// Maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.
    /// </summary>
    public interface IUnitOfWork
    {
        void Save();

        Task SaveAsync();
    }
}