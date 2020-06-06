using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;

namespace DomainFramework.Tests
{
    class ClassCommandRepository : DataAccess.EntityCommandRepository<ClassEntity>
    {
        protected override Command CreateInsertCommand(ClassEntity entity, IAuthenticatedUser user, string selector)
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

        protected override Command CreateUpdateCommand(ClassEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Class_Update")
                .AutoGenerateParameters(
                    qbeObject: entity
                );
        }
    }
}
