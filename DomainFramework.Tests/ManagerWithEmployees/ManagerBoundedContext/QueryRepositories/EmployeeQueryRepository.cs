using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class EmployeeQueryRepository : EntityQueryRepository<Employee, int?>
    {
        public override Employee GetById(int? employeeId, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Employee> GetByIdAsync(int? employeeId, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Employee> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pEmployee_Get]")
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Employee>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pEmployee_Get]")
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Employee> GetEmployeesForManager(int? supervisorId, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pEmployee_GetEmployees_ForManager]")
                .Parameters(
                    p => p.Name("supervisorId").Value(supervisorId.Value)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForManagerAsync(int? supervisorId, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pEmployee_GetEmployees_ForManager]")
                .Parameters(
                    p => p.Name("supervisorId").Value(supervisorId.Value)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Employee>(new EmployeeQueryRepository());
        }

    }
}