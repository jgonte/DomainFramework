using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace PersonWithSpouseAndDependants.PersonBoundedContext
{
    public class PersonCommandRepository : EntityCommandRepository<Person>
    {
        protected override Command CreateInsertCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<Person>
                .Single()
                .Connection(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var spouseDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "Spouse")?.Entity;

                    if (spouseDependency != null)
                    {
                        entity.MarriedPersonId = spouseDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("marriedPersonId").Value(entity.MarriedPersonId)
                    );

                    var dependantsDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "Dependants")?.Entity;

                    if (dependantsDependency != null)
                    {
                        entity.ProviderPersonId = dependantsDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("providerPersonId").Value(entity.ProviderPersonId)
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
            ((SingleQuery<Person>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Person>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Update]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("providerPersonId").Value(entity.ProviderPersonId)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    cmd.Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                    var entityForSpouse = (Person)dependencies.SingleOrDefault(d => d.Selector == "Spouse")?.Entity;

                    if (entityForSpouse != null)
                    {
                        entity.MarriedPersonId = entityForSpouse.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("marriedPersonId").Value(entity.MarriedPersonId)
                    );
                });
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

        protected override Command CreateDeleteCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Delete]")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id)
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

        protected override Command CreateDeleteLinksCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "UnlinkDependantsFromPerson": return Command
                    .NonQuery()
                    .Connection(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName())
                    .StoredProcedure("[PersonBoundedContext].[pPerson_UnlinkDependants]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                case "UnlinkSpouseFromPerson": return Command
                    .NonQuery()
                    .Connection(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName())
                    .StoredProcedure("[PersonBoundedContext].[pPerson_UnlinkSpouse]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                default: throw new InvalidOperationException();
            }
        }

        protected override bool HandleDeleteLinks(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteLinksAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}