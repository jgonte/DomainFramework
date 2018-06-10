using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PersonCommandRepository : DataAccess.CommandRepository<PersonEntity, int?>
    {
        protected override Command CreateInsertCommand(PersonEntity entity, IAuthenticatedUser user)
        {
            return Query<PersonEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<PersonEntity, object>>[]{
                        m => m.Id,
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(PersonEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Update")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id.Value)
                )
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<PersonEntity, object>>[]{
                        m => m.Id,
                    }
                );
        }

        protected override Command CreateDeleteCommand(PersonEntity entity, IAuthenticatedUser user)
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
            throw new System.NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}