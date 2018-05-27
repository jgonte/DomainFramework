using DomainFramework.Core;

namespace DomainFramework.DataAccess
{
    public class RepositoryContext : Core.RepositoryContext
    {
<<<<<<< HEAD
        public RepositoryContext(string connectionName)
        {
            ConnectionName = connectionName;
=======
        private string _connectionName;

        public RepositoryContext(string connectionName)
        {
            _connectionName = connectionName;
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }

        public override IUnitOfWork CreateUnitOfWork()
        {
<<<<<<< HEAD
            return new UnitOfWork(ConnectionName);
=======
            return new UnitOfWork(_connectionName);
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        }
    }
}
