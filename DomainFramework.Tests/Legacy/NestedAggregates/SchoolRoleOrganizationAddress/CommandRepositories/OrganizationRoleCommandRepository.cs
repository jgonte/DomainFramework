using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class OrganizationRoleCommandRepository : EntityCommandRepository<OrganizationRole>
    {
        protected override Command CreateInsertCommand(OrganizationRole entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_Insert]")
                .Parameters(
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    if (!dependencies.Any())
                    {
                        cmd.Parameters(
                            p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                            p => p.Name("roleId").Value(entity.Id.RoleId)
                        );
                    }
                    else
                    {
                        var organization = (Organization)dependencies.ElementAt(0).Entity;

                        var role = (Role)dependencies.ElementAt(1).Entity;

                        entity.Id = new OrganizationRoleId
                        {
                            OrganizationId = organization.Id.Value,
                            RoleId = role.Id.Value
                        };

                        cmd.Parameters(
                            p => p.Name("organizationId").Value(organization.Id),
                            p => p.Name("roleId").Value(role.Id)
                        );
                    }
                });

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((NonQueryCommand)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(OrganizationRole entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.LastUpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_Update]")
                .Parameters(
                    p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                    p => p.Name("roleId").Value(entity.Id.RoleId),
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

        protected override Command CreateDeleteCommand(OrganizationRole entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pOrganizationRole_Delete]")
                .Parameters(
                    p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                    p => p.Name("roleId").Value(entity.Id.RoleId)
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

        protected override Command CreateDeleteCollectionCommand(OrganizationRole entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "Role": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pOrganizationRole_DeleteRole]")
                    .Parameters(
                        p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                        p => p.Name("roleId").Value(entity.Id.RoleId)
                    );

                case "Organization": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pOrganizationRole_DeleteOrganization]")
                    .Parameters(
                        p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                        p => p.Name("roleId").Value(entity.Id.RoleId)
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
