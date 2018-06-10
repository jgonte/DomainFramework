using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class PhotoCommandRepository : DataAccess.CommandRepository<PhotoEntity, int?>
    {
        protected override Command CreateInsertCommand(PhotoEntity entity, IAuthenticatedUser user)
        {
            var command = Query<PhotoEntity>
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
                    pm => pm.Map(m => m.Id)//.Index(0),
                );

            command.OnBeforeCommandExecuted(() =>
            {
                var e = (UserEntity)TransferEntities().Single();

                entity.UserId = e.Id.Value;

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("userId").Value(entity.UserId)
                );
            });

            return command;
        }

        protected override Command CreateUpdateCommand(PhotoEntity entity, IAuthenticatedUser user)
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

        protected override Command CreateDeleteCommand(PhotoEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<PhotoEntity>)command).Execute();
        }

        protected override bool HandleUpdate(Command commandy)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}