using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class ClassCommandRepository : DataAccess.EntityCommandRepository<ClassEntity>
    {
        protected override Command CreateInsertCommand(ClassEntity entity, IAuthenticatedUser user)
        {
            return Query<ClassEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<ClassEntity, object>>[]{
                        m => m.Id,
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<ClassEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(ClassEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_Update")
                .AutoGenerateParameters(
                    qbeObject: entity
                );
        }

        protected override Command CreateDeleteCommand(ClassEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
