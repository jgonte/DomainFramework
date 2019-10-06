using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerCommandRepository : EntityCommandRepository<Manager>
    {
        protected override Command CreateInsertCommand(Manager entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Manager>
                .Single()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Insert]")
                .Parameters(
                    p => p.Name("department").Value(entity.Department),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("supervisorId").Value(entity.SupervisorId)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Manager>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Manager>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Manager entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Update]")
                .Parameters(
                    p => p.Name("managerId").Value(entity.Id),
                    p => p.Name("department").Value(entity.Department),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("supervisorId").Value(entity.SupervisorId)
                );
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleUpdateAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

        protected override Command CreateDeleteCommand(Manager entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_Delete]")
                .Parameters(
                    p => p.Name("managerId").Value(entity.Id)
                );
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

        protected override Command CreateDeleteCollectionCommand(Manager entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ManagerWithEmployeesConnectionClass.GetConnectionName())
                .StoredProcedure("[ManagerBoundedContext].[pManager_DeleteEmployees]")
                .Parameters(
                    p => p.Name("supervisorId").Value(entity.Id)
                );
        }

        protected override bool HandleDeleteCollection(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteCollectionAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}