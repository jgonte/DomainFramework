using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TestEntityCommandRepository : EntityCommandRepository<TestEntity>
    {
        protected override Command CreateInsertCommand(TestEntity entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.CreatedBy = (int)user.Id;
            }

            var command = Query<TestEntity>
                .Single()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_Insert]")
                .Parameters(
                    p => p.Name("text").Value(entity.Text),
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
            ((SingleQuery<TestEntity>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<TestEntity>)command).ExecuteAsync();
        }

        protected override Command CreateUpdateCommand(TestEntity entity, IAuthenticatedUser user, string selector)
        {
            if (user != null)
            {
                entity.UpdatedBy = (int)user.Id;
            }

            return Command
                .NonQuery()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_Update]")
                .Parameters(
                    p => p.Name("testEntityId").Value(entity.Id),
                    p => p.Name("text").Value(entity.Text),
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

        protected override Command CreateDeleteCommand(TestEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_Delete]")
                .Parameters(
                    p => p.Name("testEntityId").Value(entity.Id)
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

        protected override Command CreateDeleteLinksCommand(TestEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_DeleteTypeValues1]")
                .ThrowWhenNoRecordIsUpdated(false)
                .Parameters(
                    p => p.Name("testEntityId").Value(entity.Id)
                );
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