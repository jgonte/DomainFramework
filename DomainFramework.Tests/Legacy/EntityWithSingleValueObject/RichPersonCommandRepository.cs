using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq.Expressions;

namespace DomainFramework.Tests.EntityWithValueObjects
{
    class RichPersonCommandRepository : EntityCommandRepository<RichPersonEntity>
    {
        protected override Command CreateInsertCommand(RichPersonEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<RichPersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_RichPerson_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<RichPersonEntity, object>>[]{
                        m => m.Id,
                        m => m.Capital
                    }
                )
                .Parameters(
                    p => p.Name("moneyAmount").Value(entity.Capital.Value)
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<RichPersonEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(RichPersonEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_RichPerson_Update")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<RichPersonEntity, object>>[]{
                        m => m.Id,
                        m => m.Capital
                    }
                )
                .Parameters(
                    p => p.Name("richPersonId").Value(entity.Id.Value),
                    p => p.Name("moneyAmount").Value(entity.Capital.Value)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<RichPersonEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

    }
}