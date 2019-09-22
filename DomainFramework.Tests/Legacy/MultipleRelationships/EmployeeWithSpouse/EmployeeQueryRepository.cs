using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class EmployeeQueryRepository : EntityQueryRepository<Employee, int?>
    {
        public override Employee GetById(int? employeeId, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Employee> GetByIdAsync(int? employeeId, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Employee> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Employee>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pEmployee_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Employee>(new EmployeeQueryRepository());
        }

    }
}
