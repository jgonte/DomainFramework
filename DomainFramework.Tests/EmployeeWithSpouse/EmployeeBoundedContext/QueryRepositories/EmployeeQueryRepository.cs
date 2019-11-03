using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class EmployeeQueryRepository : EntityQueryRepository<Employee, int?>
    {
        public override Employee GetById(int? employeeId, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Single()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_GetById]")
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
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override (int, IEnumerable<Employee>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Employee>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Employee>(new EmployeeQueryRepository());
        }

    }
}