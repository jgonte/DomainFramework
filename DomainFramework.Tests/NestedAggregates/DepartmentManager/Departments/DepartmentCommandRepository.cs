using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class DepartmentCommandRepository : DataAccess.EntityCommandRepository<DepartmentEntity>
    {
        protected override Command CreateInsertCommand(DepartmentEntity entity, IAuthenticatedUser user)
        {
            return Query<DepartmentEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Department_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<DepartmentEntity, object>>[]{
                        m => m.Id
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<DepartmentEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(DepartmentEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Department_Update")
                .Parameters(
                    p => p.Name("departmentId").Value(entity.Id.Value)
                )
                .AutoGenerateParameters(
                    qbeObject: entity
                );
        }

        protected override Command CreateDeleteCommand(DepartmentEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Department_Delete")
                .Parameters(
                    p => p.Name("bookId").Value(entity.Id.Value)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<DepartmentEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}