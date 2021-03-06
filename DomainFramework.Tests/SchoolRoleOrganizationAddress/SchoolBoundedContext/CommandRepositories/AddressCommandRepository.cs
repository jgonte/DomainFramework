using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

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
                .RecordInstance(entity)
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

        protected override Command CreateUpdateCommand(Address entity, IAuthenticatedUser user, string selector)
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

        protected override Command CreateDeleteCommand(Address entity, IAuthenticatedUser user, string selector)
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

    }
}