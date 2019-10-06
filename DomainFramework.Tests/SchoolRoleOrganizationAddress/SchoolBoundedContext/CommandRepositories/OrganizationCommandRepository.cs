using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationCommandRepository : EntityCommandRepository<Organization>
    {
        protected override Command CreateInsertCommand(Organization entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Organization>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy),
                    p => p.Name("phone_Number").Value(entity.Phone.Number)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var e = (Address)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("addressId").Value(e.Id)
                    );
                })
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Organization>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Organization>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Organization entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_Update]")
                .Parameters(
                    p => p.Name("organizationId").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("phone_Number").Value(entity.Phone.Number),
                    p => p.Name("addressId").Value(entity.AddressId)
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

        protected override Command CreateDeleteCommand(Organization entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_Delete]")
                .Parameters(
                    p => p.Name("organizationId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Organization entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pOrganization_DeleteRole]")
                .Parameters(
                    p => p.Name("organizationId").Value(entity.Id)
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