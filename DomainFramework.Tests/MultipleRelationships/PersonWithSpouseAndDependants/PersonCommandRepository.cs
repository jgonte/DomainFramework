using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.PersonWithSpouseAndDependants
{
    public class PersonCommandRepository : EntityCommandRepository<Person>
    {
        protected override Command CreateInsertCommand(Person entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            return Query<Person>
                .Single()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entityForSpouse = (Person)dependencies.SingleOrDefault(d => d.Selector == "Spouse")?.Entity;

                    if (entityForSpouse != null)
                    {
                        cmd.Parameters(
                            p => p.Name("marriedPersonId").Value(entityForSpouse.Id)
                        );
                    }
                    else
                    {
                        cmd.Parameters(
                            p => p.Name("marriedPersonId").Value(entity.MarriedPersonId)
                        );
                    }

                    var entityForDependants = (Person)dependencies.SingleOrDefault(d => d.Selector == "Dependants")?.Entity;

                    if (entityForDependants != null)
                    {
                        cmd.Parameters(
                            p => p.Name("providerPersonId").Value(entityForDependants.Id)
                        );
                    }
                    else
                    {
                        cmd.Parameters(
                            p => p.Name("providerPersonId").Value(entity.ProviderPersonId)
                        );
                    }
                })
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Person>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<Person>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(Person entity, IAuthenticatedUser user)
        {
            if (user != null)
            {
                entity.LastUpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Update]")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("lastUpdatedBy").Value(entity.LastUpdatedBy),
                    p => p.Name("marriedPersonId").Value(entity.MarriedPersonId),
                    p => p.Name("providerPersonId").Value(entity.ProviderPersonId)
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

        protected override Command CreateDeleteCommand(Person entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(TestConnectionClass.GetConnectionName())
                .StoredProcedure("[pPerson_Delete]")
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

        protected override Command CreateDeleteCollectionCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "Spouse": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pPerson_DeleteSpouse]")
                    .Parameters(
                        p => p.Name("marriedPersonId").Value(entity.Id)
                    );

                case "Dependants": return Command
                    .NonQuery()
                    .Connection(TestConnectionClass.GetConnectionName())
                    .StoredProcedure("[pPerson_DeleteDependants]")
                    .Parameters(
                        p => p.Name("providerPersonId").Value(entity.Id)
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
