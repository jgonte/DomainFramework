using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class AddressCommandRepository : EntityCommandRepository<Address>
    {
        protected override Command CreateInsertCommand(Address entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Address>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_Insert]")
                .Parameters(
                    p => p.Name("street").Value(entity.Street),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var addressDependency = dependencies?.SingleOrDefault(d => d.Selector == "Address")?.Entity;

                    if (addressDependency is Organization)
                    {
                        var organizationDependency = (Organization)addressDependency;

                        cmd.Parameters(
                            p => p.Name("organizationId").Value(organizationDependency.Id)
                        );
                    }
                    else if (addressDependency is Person)
                    {
                        var personDependency = (Person)addressDependency;

                        cmd.Parameters(
                            p => p.Name("personId").Value(personDependency.Id)
                        );
                    }
                })
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

        protected override Command CreateUpdateCommand(Address entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_Update]")
                .Parameters(
                    p => p.Name("addressId").Value(entity.Id),
                    p => p.Name("street").Value(entity.Street),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("organizationId").Value(entity.OrganizationId),
                    p => p.Name("personId").Value(entity.PersonId)
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
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pAddress_Delete]")
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