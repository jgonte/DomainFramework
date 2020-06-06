using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class ExecutiveCommandRepository : DataAccess.EntityCommandRepository<ExecutiveEntity>
    {
        protected override Command CreateInsertCommand(ExecutiveEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Executive_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<ExecutiveEntity, object>>[]{
                        m => m.Id
                    }
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entities = Dependencies();

                    var employeeEntity = (EmployeeEntity)entities.Single().Entity;

                    entity.Id = employeeEntity.Id;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("executiveId").Value(entity.Id)
                    );
                });

        }

        protected override Command CreateUpdateCommand(ExecutiveEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Executive_Update")
                .Parameters(
                    p => p.Name("executiveId").Value(entity.Id.Value)
                )
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<ExecutiveEntity, object>>[]{
                        m => m.Id
                    }
                );
        }

        protected override Command CreateDeleteCommand(ExecutiveEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Executive_Delete")
                .Parameters(
                    p => p.Name("executiveId").Value(entity.Id.Value)
                );
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected override async Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }
    }
}