using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class EmployeeQueryRepository : QueryRepository<EmployeeEntity, int?>
    {
        public override IEnumerable<IEntity> Get(QueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<IEnumerable<IEntity>> GetAsync(QueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override EmployeeEntity GetById(int? id)
        {
            var result = Query<EmployeeEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Get")
                .Parameters(
                    p => p.Name("employeeId").Value(id.Value)
                )
                .Execute();

            return result.Data;
        }

        public override async Task<EmployeeEntity> GetByIdAsync(int? id)
        {
            var result = await Query<EmployeeEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Employee_Get")
                .Parameters(
                    p => p.Name("employeeId").Value(id.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }
    }
}