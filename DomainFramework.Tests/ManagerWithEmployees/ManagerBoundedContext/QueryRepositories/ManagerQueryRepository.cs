using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerQueryRepository : EntityQueryRepository<Manager, int>
    {
        public override (int, IEnumerable<Manager>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Manager>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Manager>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Manager>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Manager> GetAll()
        {
            var result = Query<Manager>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Manager>> GetAllAsync()
        {
            var result = await Query<Manager>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Manager GetById(int managerId)
        {
            var result = Query<Manager>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetById]")
                .Parameters(
                    p => p.Name("managerId").Value(managerId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Manager> GetByIdAsync(int managerId)
        {
            var result = await Query<Manager>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetById]")
                .Parameters(
                    p => p.Name("managerId").Value(managerId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public IEnumerable<Employee> GetAllEmployeesForManager(int managerId)
        {
            var result = Query<Employee>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetAllEmployees]")
                .Parameters(
                    p => p.Name("managerId").Value(managerId)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .Execute();

            return result.Records;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesForManagerAsync(int managerId)
        {
            var result = await Query<Employee>
                .Collection()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_GetAllEmployees]")
                .Parameters(
                    p => p.Name("managerId").Value(managerId)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Manager)).Index(1),
                    tm => tm.Type(typeof(Employee)).Index(2)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Manager>(new ManagerQueryRepository());
        }

    }
}