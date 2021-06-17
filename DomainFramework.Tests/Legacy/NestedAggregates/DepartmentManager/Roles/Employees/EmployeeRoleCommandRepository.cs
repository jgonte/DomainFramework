using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class EmployeeRoleCommandRepository : DataAccess.EntityCommandRepository<EmployeeRoleEntity>
    {
        protected override Command CreateInsertCommand(EmployeeRoleEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<EmployeeRoleEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_EmployeeRole_Create")
                .RecordInstance(entity)
                .AutoGenerateParameters(
                    excludedProperties: new Expression<Func<EmployeeRoleEntity, object>>[]{
                        m => m.Id,
                        m => m.DepartmentId
                    }
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entities = Dependencies();

                    var departmentEntity = (DepartmentEntity)entities.ElementAt(0).Entity;

                    var personEntity = (PersonEntity)entities.ElementAt(1).Entity;

                    entity.Id = personEntity.Id;

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("departmentId").Value(departmentEntity.Id),
                        p => p.Name("personId").Value(personEntity.Id)
                    );
                });

        }

        protected override Command CreateUpdateCommand(EmployeeRoleEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_EmployeeRole_Update")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id.Value)
                )
                .RecordInstance(entity)
                .AutoGenerateParameters();
        }

        protected override Command CreateDeleteCommand(EmployeeRoleEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_EmployeeRole_Delete")
                .Parameters(
                    p => p.Name("employeeRoleId").Value(entity.Id.Value)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<EmployeeRoleEntity>)command).Execute();
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
    }
}