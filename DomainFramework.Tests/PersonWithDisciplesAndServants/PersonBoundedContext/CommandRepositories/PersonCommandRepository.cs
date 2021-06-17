using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace PersonWithDisciplesAndServants.PersonBoundedContext
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
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("gender").Value(entity.Gender),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var disciplesDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "Disciples")?.Entity;

                    if (disciplesDependency != null)
                    {
                        entity.LeaderId = disciplesDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("leaderId").Value(entity.LeaderId)
                    );

                    var servantsDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "Servants")?.Entity;

                    if (servantsDependency != null)
                    {
                        entity.MasterId = servantsDependency.Id;
                    }

                    cmd.Parameters(
                        p => p.Name("masterId").Value(entity.MasterId)
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
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                .StoredProcedure("[PersonBoundedContext].[pPerson_Update]")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("gender").Value(entity.Gender),
                    p => p.Name("updatedBy").Value(entity.UpdatedBy),
                    p => p.Name("leaderId").Value(entity.LeaderId),
                    p => p.Name("masterId").Value(entity.MasterId)
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

        protected override Command CreateDeleteCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
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
                case "UnlinkDisciplesFromPerson": return Command
                    .NonQuery()
                    .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                    .StoredProcedure("[PersonBoundedContext].[pPerson_UnlinkDisciples]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                case "UnlinkServantsFromPerson": return Command
                    .NonQuery()
                    .Connection(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName())
                    .StoredProcedure("[PersonBoundedContext].[pPerson_UnlinkServants]")
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