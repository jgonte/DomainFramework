using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class PhoneCommandRepository : EntityCommandRepository<Phone>
    {
        protected override Command CreateInsertCommand(Phone entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Phone>
                .Single()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_Insert]")
                .Parameters(
                    p => p.Name("number").Value(entity.Number),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var phonesDependency = dependencies?.SingleOrDefault(d => d.Selector == "Phones")?.Entity;

                    if (phonesDependency is Organization)
                    {
                        var organizationDependency = (Organization)phonesDependency;

                        cmd.Parameters(
                            p => p.Name("organizationId").Value(organizationDependency.Id)
                        );
                    }
                    else if (phonesDependency is Person)
                    {
                        var personDependency = (Person)phonesDependency;

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
            ((SingleQuery<Phone>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Phone>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Phone entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_Update]")
                .Parameters(
                    p => p.Name("phoneId").Value(entity.Id),
                    p => p.Name("number").Value(entity.Number),
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

        protected override Command CreateDeleteCommand(Phone entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPhone_Delete]")
                .Parameters(
                    p => p.Name("phoneId").Value(entity.Id)
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