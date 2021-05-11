using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class TestEntity_TypeValues1_CommandRepository : LinkedValueObjectCommandRepository<TypeValue>
    {
        protected override Command CreateInsertCommand(TypeValue valueObject, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_AddTypeValues1]")
                .Parameters(
                    p => p.Name("dataType").Value(valueObject.DataType),
                    p => p.Name("data").Value(valueObject.Data)
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (TestEntity)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("testEntityId").Value(entity.Id)
                    );
                });
        }

        protected override void HandleInsert(Command command)
        {
            ((NonQueryCommand)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((NonQueryCommand)command).ExecuteAsync();
        }

        protected override Command CreateDeleteLinksCommand(IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName())
                .StoredProcedure("[pTestEntity_DeleteTypeValues1]")
                .ThrowWhenNoRecordIsUpdated(false)
                .OnBeforeCommandExecuted(cmd =>
                {
                    var dependencies = Dependencies();

                    var entity = (TestEntity)dependencies.Single().Entity;

                    cmd.Parameters(
                        p => p.Name("testEntityId").Value(entity.Id)
                    );
                });
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

        public static void RegisterFactory(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterCommandRepositoryFactory<RepositoryKey>(() => new TestEntity_TypeValues1_CommandRepository());
        }

        public class RepositoryKey
        {
        }
    }
}