using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EntityWithValueObjects
{
    class RichPersonCommandRepository : DataAccess.EntityCommandRepository<RichPersonEntity>
    {
        protected override Command CreateInsertCommand(RichPersonEntity entity, IAuthenticatedUser user)
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

        protected override Command CreateUpdateCommand(RichPersonEntity entity, IAuthenticatedUser user)
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

        protected override Command CreateDeleteCommand(RichPersonEntity entity, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<RichPersonEntity>)command).Execute();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}