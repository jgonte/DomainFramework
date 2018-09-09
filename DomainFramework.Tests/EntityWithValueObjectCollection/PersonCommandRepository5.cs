using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class PersonCommandRepository5 : DataAccess.EntityCommandRepository<PersonEntity4>
    {
        protected override Command CreateInsertCommand(PersonEntity4 entity, IAuthenticatedUser user)
        {
            return Query<PersonEntity4>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<PersonEntity4, object>>[]{
                        m => m.Id
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<PersonEntity4>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(PersonEntity4 entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Person_Update")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<PersonEntity4, object>>[]{
                        m => m.Id,
                    }
                )
                .Parameters(
                    p => p.Name("personId").Value(entity.Id.Value)
                );
        }

        protected override Command CreateDeleteCommand(PersonEntity4 entity, IAuthenticatedUser user)
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