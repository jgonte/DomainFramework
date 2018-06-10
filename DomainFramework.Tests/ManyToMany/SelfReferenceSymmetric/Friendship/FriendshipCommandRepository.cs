using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests
{
    class FriendshipCommandRepository : DataAccess.CommandRepository<FriendshipEntity, FriendshipEntityId>
    {
        protected override Command CreateInsertCommand(FriendshipEntity entity, IAuthenticatedUser user)
        {
            var command = Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Friendship_Create")
                .AutoGenerateParameters(
                    qbeObject: entity
                );

            command.OnBeforeCommandExecuted(() =>
            {
                var entities = TransferEntities();

                var personEntity = (PersonEntity)entities.ElementAt(0);

                var friendEntity = (PersonEntity)entities.ElementAt(1);

                entity.Id = new FriendshipEntityId
                {
                    PersonId = personEntity.Id.Value,
                    FriendId = friendEntity.Id.Value
                };

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("PersonId").Value(personEntity.Id),
                    p => p.Name("FriendId").Value(friendEntity.Id)
                );
            });

            return command;
        }

        protected override Command CreateUpdateCommand(FriendshipEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override Command CreateDeleteCommand(FriendshipEntity entity, IAuthenticatedUser user)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleDelete(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleUpdate(Command command)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> HandleUpdateAsync(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
