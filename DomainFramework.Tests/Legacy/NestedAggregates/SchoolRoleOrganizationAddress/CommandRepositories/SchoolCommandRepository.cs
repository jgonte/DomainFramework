using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SchoolCommandRepository : EntityCommandRepository<School>
    {
        protected override Command CreateInsertCommand(School entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<School>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pSchool_Insert]")
                .Parameters(
                    p => p.Name("isCharter").Value(entity.IsCharter),
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
            ((SingleQuery<School>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<School>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(School entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.LastUpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pSchool_Update]")
                .Parameters(
                    p => p.Name("schoolId").Value(entity.Id),
                    p => p.Name("isCharter").Value(entity.IsCharter),
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

        protected override Command CreateDeleteCommand(School entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pSchool_Delete]")
                .Parameters(
                    p => p.Name("schoolId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(School entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "Organization": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pSchool_DeleteOrganization]")
                    .Parameters(
                        p => p.Name("roleId").Value(entity.Id)
                    );

                case "Role": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pSchool_DeleteRole]")
                    .Parameters(
                        p => p.Name("schoolId").Value(entity.Id)
                    );

                default: throw new InvalidOperationException();
            }
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