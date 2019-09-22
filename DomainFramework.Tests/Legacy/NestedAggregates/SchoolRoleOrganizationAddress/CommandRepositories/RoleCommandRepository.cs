using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class RoleCommandRepository : EntityCommandRepository<Role>
    {
        protected override Command CreateInsertCommand(Role entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Role>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pRole_Insert]")
                .Parameters(
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Role>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Role>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Role entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.LastUpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pRole_Update]")
                .Parameters(
                    p => p.Name("roleId").Value(entity.Id),
                    p => p.Name("lastUpdatedBy").Value(entity.LastUpdatedBy)
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

        protected override Command CreateDeleteCommand(Role entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pRole_Delete]")
                .Parameters(
                    p => p.Name("roleId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Role entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pRole_DeleteOrganization]")
                .Parameters(
                    p => p.Name("roleId").Value(entity.Id)
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