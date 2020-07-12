using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
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
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_Insert]")
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
                        switch (selector)
                        {
                            case "Organization":
                                {
                                    var role = (Role)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                                        p => p.Name("roleId").Value(role.Id)
                                    );
                                }
                                break;

                            case "Role":
                                {
                                    var organization = (Organization)dependencies.ElementAt(0).Entity;

                                    cmd.Parameters(
                                        p => p.Name("organizationId").Value(organization.Id),
                                        p => p.Name("roleId").Value(entity.Id.RoleId)
                                    );
                                }
                                break;

                            default:
                                {
                                    var organization = (Organization)dependencies.Single(d => d.Selector == "Organization").Entity;

                                    var role = (Role)dependencies.Single(d => d.Selector == "Role").Entity;

                                    entity.Id = new OrganizationRoleId
                                    {
                                        OrganizationId = organization.Id,
                                        RoleId = role.Id
                                    };

                                    cmd.Parameters(
                                        p => p.Name("organizationId").Value(organization.Id),
                                        p => p.Name("roleId").Value(role.Id)
                                    );
                                }
                                break;
                        }
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

        protected override Command CreateUpdateCommand(OrganizationRole entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_Update]")
                .Parameters(
                    p => p.Name("organizationId").Value(entity.Id.OrganizationId),
                    p => p.Name("roleId").Value(entity.Id.RoleId),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
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

        protected override Command CreateDeleteCommand(OrganizationRole entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganizationRole_Delete]")
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

    }
}