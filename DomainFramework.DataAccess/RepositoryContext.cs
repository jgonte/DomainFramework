using DomainFramework.Core;

namespace DomainFramework.DataAccess
{
    public class RepositoryContext : Core.RepositoryContext
    {
        public RepositoryContext(string connectionName)
        {
            ConnectionName = connectionName;
        }

        public override IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(ConnectionName);
        }
    }
}
