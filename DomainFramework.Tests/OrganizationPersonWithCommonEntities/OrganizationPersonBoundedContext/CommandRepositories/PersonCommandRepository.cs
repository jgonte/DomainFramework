using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
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
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_Insert]")
                .Parameters(
                    p => p.Name("name").Value(entity.Name),
                    p => p.Name("createdBy").Value(entity.CreatedBy)
                )
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
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_Update]")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id),
                    p => p.Name("name").Value(entity.Name),
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

        protected override Command CreateDeleteCommand(Person entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_Delete]")
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
                case "UnlinkAddressFromPerson": return Command
                    .NonQuery()
                    .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                    .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_UnlinkAddress]")
                    .ThrowWhenNoRecordIsUpdated(false)
                    .Parameters(
                        p => p.Name("personId").Value(entity.Id)
                    );

                case "UnlinkPhonesFromPerson": return Command
                    .NonQuery()
                    .Connection(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName())
                    .StoredProcedure("[OrganizationPersonBoundedContext].[pPerson_UnlinkPhones]")
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