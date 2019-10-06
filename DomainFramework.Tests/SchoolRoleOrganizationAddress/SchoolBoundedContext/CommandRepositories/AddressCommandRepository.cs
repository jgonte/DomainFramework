using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class AddressCommandRepository : EntityCommandRepository<Address>
    {
        protected override Command CreateInsertCommand(Address entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<Address>
                .Single()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Insert]")
                .Parameters(
                    p => p.Name("street").Value(entity.Street)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Address>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Address>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Address entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Update]")
                .Parameters(
                    p => p.Name("addressId").Value(entity.Id),
                    p => p.Name("street").Value(entity.Street)
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

        protected override Command CreateDeleteCommand(Address entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                .StoredProcedure("[SchoolBoundedContext].[pAddress_Delete]")
                .Parameters(
                    p => p.Name("addressId").Value(entity.Id)
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

        protected override Command CreateDeleteCollectionCommand(Address entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "Role": return Command
                    .NonQuery()
                    .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                    .StoredProcedure("[SchoolBoundedContext].[pAddress_DeleteRole]")
                    .Parameters(
                        p => p.Name("addressId").Value(entity.Id)
                    );

                case "Organization": return Command
                    .NonQuery()
                    .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                    .StoredProcedure("[SchoolBoundedContext].[pAddress_DeleteOrganization]")
                    .Parameters(
                        p => p.Name("addressId").Value(entity.Id)
                    );

                case "Organizations": return Command
                    .NonQuery()
                    .Connection(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName())
                    .StoredProcedure("[SchoolBoundedContext].[pAddress_DeleteOrganizations]")
                    .Parameters(
                        p => p.Name("addressId").Value(entity.Id)
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