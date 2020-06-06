using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class EmployeeQueryRepository : EntityQueryRepository<Employee, int?>
    {
        public override (int, IEnumerable<Employee>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(EmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Employee>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(EmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override IEnumerable<Employee> GetAll()
        {
            var result = Query<Employee>
                .Collection()
                .Connection(EmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_GetAll]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Employee>> GetAllAsync()
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(EmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Employee GetById(int? employeeId)
        {
            var result = Query<Employee>
                .Single()
                .Connection(EmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Employee> GetByIdAsync(int? employeeId)
        {
            var result = await Query<Employee>
                .Single()
                .Connection(EmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pEmployee_GetById]")
                .Parameters(
                    p => p.Name("employeeId").Value(employeeId.Value)
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