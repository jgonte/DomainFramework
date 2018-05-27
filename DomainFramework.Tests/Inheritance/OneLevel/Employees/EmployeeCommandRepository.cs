using System.Threading.Tasks;
using DataAccess;
using DomainFramework.DataAccess;

namespace DomainFramework.Tests.OneLevelInheritance
{
    class EmployeeCommandRepository : CommandRepository<EmployeeEntity, int?>
    {
        protected override Command CreateDeleteCommand(EmployeeEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override Command CreateInsertCommand(EmployeeEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override Command CreateUpdateCommand(EmployeeEntity entity)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}