using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class UserCommandRepository : DataAccess.EntityCommandRepository<UserEntity>
    {
        protected override Command CreateInsertCommand(UserEntity entity, IAuthenticatedUser user)
        {
            return Query<UserEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_User_Create")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<UserEntity, object>>[]{
                        m => m.Id,
                        m => m.DefaultPhotoId
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<UserEntity>(m => m.Id)//.Index(0),
                );
        }

        protected override Command CreateUpdateCommand(UserEntity entity, IAuthenticatedUser user)
        {
            Expression<Func<UserEntity, object>>[] excludedProperties;

            if (Dependencies != null)
            {
                excludedProperties = new Expression<Func<UserEntity, object>>[]{
                    m => m.Id,
                    m => m.DefaultPhotoId
                };
            }
            else
            {
                excludedProperties = new Expression<Func<UserEntity, object>>[]{
                    m => m.Id
                };
            }

            return Query<UserEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_User_Update")
                .AutoGenerateParameters(
                    qbeObject: entity,
                    excludedProperties: excludedProperties
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<UserEntity>(m => m.Id)//.Index(0),
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    if (Dependencies != null)
                    {
                        var userEntity = (UserEntity)Dependencies().ElementAt(0);

                        var defaultPhotoEntity = (PhotoEntity)Dependencies().ElementAt(1);

                        entity.Id = userEntity.Id;

                        entity.DefaultPhotoId = defaultPhotoEntity.Id;

                        cmd.Parameters( // Map the extra parameters for the foreign key(s)
                            p => p.Name("userId").Value(userEntity.Id),
                            p => p.Name("defaultPhotoId").Value(defaultPhotoEntity.Id)
                        );
                    }
                });

        }

        protected override Command CreateDeleteCommand(UserEntity entity, IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_User_Delete")
                .Parameters(
                    p => p.Name("userId").Value(entity.Id.Value)
                );
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<UserEntity>)command).Execute();
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