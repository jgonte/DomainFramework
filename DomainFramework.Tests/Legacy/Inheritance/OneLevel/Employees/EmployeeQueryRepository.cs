using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class EmployeeQueryRepository : EntityQueryRepository<EmployeeEntity, int?>
    {
        public override (int, IEnumerable<EmployeeEntity>) Get(CollectionQueryParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public override Task<(int, IEnumerable<EmployeeEntity>)> GetAsync(CollectionQueryParameters parameters)
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

            return result.Record;
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

            return result.Record;
        }
    }
}