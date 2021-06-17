using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
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
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("gender").Value(entity.Gender),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var marriedToDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "MarriedTo")?.Entity;

                    if (marriedToDependency != null)
                    {
                        entity.SpouseId = marriedToDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("spouseId").Value(entity.SpouseId)
                    );

                    var bestFriendOfDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "BestFriendOf")?.Entity;

                    if (bestFriendOfDependency != null)
                    {
                        entity.BestFriendId = bestFriendOfDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("bestFriendId").Value(entity.BestFriendId)
                    );
                })
                .RecordInstance(entity)
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
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Update]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("gender").Value(entity.Gender),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    cmd.Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                    var entityForMarriedTo = (Person)dependencies.SingleOrDefault(d => d.Selector == "MarriedTo")?.Entity;

                    if (entityForMarriedTo != null)
                    {
                        entity.SpouseId = entityForMarriedTo.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("spouseId").Value(entity.SpouseId)
                    );

                    var entityForBestFriendOf = (Person)dependencies.SingleOrDefault(d => d.Selector == "BestFriendOf")?.Entity;

                    if (entityForBestFriendOf != null)
                    {
                        entity.BestFriendId = entityForBestFriendOf.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("bestFriendId").Value(entity.BestFriendId)
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
                .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
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
                case "UnlinkBestFriendOfFromPerson": return Command
                    .NonQuery()
                    .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                    .StoredProcedure("[PersonBoundedContext].[pPerson_UnlinkBestFriendOf]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                case "UnlinkMarriedToFromPerson": return Command
                    .NonQuery()
                    .Connection(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName())
                    .StoredProcedure("[PersonBoundedContext].[pPerson_UnlinkMarriedTo]")
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