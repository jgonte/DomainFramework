using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class DepartmentManagerRoleCommandRepository : DataAccess.EntityCommandRepository<DepartmentManagerRoleEntity>
    {
        protected override Command CreateInsertCommand(DepartmentManagerRoleEntity entity, IAuthenticatedUser user, string selector)
        {
            Expression<Func<DepartmentManagerRoleEntity, object>>[] excludedProperties;

            if (Dependencies().Any())
            {
                excludedProperties = new Expression<Func<DepartmentManagerRoleEntity, object>>[]{
                    m => m.Id,
                    m => m.EmployeeRoleId,
                    m => m.ManagesDepartmentId
                };
            }
            else
            {
                excludedProperties = new Expression<Func<DepartmentManagerRoleEntity, object>>[]{
                    m => m.Id
                };
            }

            return Query<DepartmentManagerRoleEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_DepartmentManagerRole_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: excludedProperties
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<DepartmentManagerRoleEntity>(m => m.Id)//.Index(0),
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    if (Dependencies().Any())
                    {
                        var employeeEntity = (EmployeeRoleEntity)Dependencies().ElementAt(0).Entity;

                        entity.EmployeeRoleId = employeeEntity.Id.Value;

                        var departmentEntity = (DepartmentEntity)Dependencies().ElementAt(1).Entity;

                        entity.ManagesDepartmentId = departmentEntity.Id.Value;

                        cmd.Parameters( // Map the extra parameters for the foreign key(s)
                            p => p.Name("employeeRoleId").Value(entity.EmployeeRoleId),
                            p => p.Name("managesDepartmentId").Value(entity.ManagesDepartmentId)
                        );
                    }
                });
        }

        protected override Command CreateUpdateCommand(DepartmentManagerRoleEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_DepartmentManagerRole_Update")
                .Parameters(
                    p => p.Name("employeeRoleId").Value(entity.Id.Value)
                )
                .AutoGenerateParameters(
                    qbeObject: entity
                );
        }

        protected override Command CreateDeleteCommand(DepartmentManagerRoleEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_DepartmentManagerRole_Delete")
                .Parameters(
                    p => p.Name("employeeRoleId").Value(entity.Id.Value)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<DepartmentManagerRoleEntity>)command).Execute();
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