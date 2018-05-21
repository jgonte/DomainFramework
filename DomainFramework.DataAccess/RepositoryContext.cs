using DomainFramework.Core;

namespace DomainFramework.DataAccess
{
    public class RepositoryContext : Core.RepositoryContext
    {
        private string _connectionName;

        public RepositoryContext(string connectionName)
        {
            _connectionName = connectionName;
        }

        public override IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_connectionName);
        }
    }
}
