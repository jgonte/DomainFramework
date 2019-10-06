using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class EmployeeQueryRepository : EntityQueryRepository<Employee, int?>
    {
        public override Employee GetById(int? employeeId, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Executive)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Employee> GetByIdAsync(int? employeeId, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Executive)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Employee> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pEmployee_Get]")
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Executive)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Employee>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pEmployee_Get]")
                .MapTypes(
                    5,
                    tm => tm.Type(typeof(Executive)).Index(1),
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