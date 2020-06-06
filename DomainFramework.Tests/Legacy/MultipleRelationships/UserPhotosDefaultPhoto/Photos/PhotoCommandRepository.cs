using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PhotoCommandRepository : DataAccess.EntityCommandRepository<PhotoEntity>
    {
        protected override Command CreateInsertCommand(PhotoEntity entity, IAuthenticatedUser user, string selector)
        {
            return Query<PhotoEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Photo_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: entity,
                    excludedProperties: new Expression<Func<PhotoEntity, object>>[]{
                        m => m.Id,
                        m => m.UserId
                    }
                )
                .Instance(entity)
                .MapProperties(
                    pm => pm.Map<PhotoEntity>(m => m.Id)//.Index(0),
                )
                .OnBeforeCommandExecuted(cmd =>
            {
                var e = (UserEntity)Dependencies().Single().Entity;

                entity.UserId = e.Id.Value;

                cmd.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("userId").Value(entity.UserId)
                );
            });
        }

        protected override Command CreateUpdateCommand(PhotoEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Photo_Update")
                .Parameters(
                    p => p.Name("photoId").Value(entity.Id.Value),
                    p => p.Name("userId").Value(entity.UserId)
                )
                .AutoGenerateParameters(
                    qbeObject: entity
                );
        }

        protected override Command CreateDeleteCommand(PhotoEntity entity, IAuthenticatedUser user, string selector)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<PhotoEntity>)command).Execute();
        }

    }
}