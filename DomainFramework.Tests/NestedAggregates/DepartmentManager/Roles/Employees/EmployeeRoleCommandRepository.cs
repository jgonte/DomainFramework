﻿using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class EmployeeRoleCommandRepository : DataAccess.CommandRepository<EmployeeRoleEntity, int?>
    {
        protected override Command CreateInsertCommand(EmployeeRoleEntity entity, IAuthenticatedUser user)
        {
            var command = Query<EmployeeRoleEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_EmployeeRole_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<EmployeeRoleEntity, object>>[]{
                        m => m.Id,
                        m => m.DepartmentId
                    }
                );

            command.OnBeforeCommandExecuted(() =>
            {
                var entities = TransferEntities();

                var departmentEntity = (DepartmentEntity)entities.ElementAt(0);

                var personEntity = (PersonEntity)entities.ElementAt(1);

                entity.Id = personEntity.Id;

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("departmentId").Value(departmentEntity.Id),
                    p => p.Name("personId").Value(personEntity.Id)
                );
            });

            return command;
        }

        protected override Command CreateUpdateCommand(EmployeeRoleEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_EmployeeRole_Update")
                .Parameters(
                    p => p.Name("personId").Value(entity.Id.Value)
                )
                .AutoGenerateParameters(
                    qbeObject: entity
                );
        }

        protected override Command CreateDeleteCommand(EmployeeRoleEntity entity, IAuthenticatedUser user)
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