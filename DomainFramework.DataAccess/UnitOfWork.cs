using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private Transaction _transaction;

        public UnitOfWork(string connectionName)
        {
            _transaction = Transaction
                .Local()
                .ReadCommitted()
                .Connection(connectionName);
        }

        public UnitOfWork Commands(params Command[] commands)
        {
            _transaction.Commands(commands);

            return this;
        }

        public void Save()
        {
            _transaction.Execute();
        }

        public async Task SaveAsync()
        {
            await _transaction.ExecuteAsync();
        }
    }
}
