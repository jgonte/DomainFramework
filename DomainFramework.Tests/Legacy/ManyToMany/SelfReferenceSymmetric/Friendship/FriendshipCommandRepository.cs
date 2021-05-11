using DataAccess;
using DomainFramework.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace DomainFramework.Tests
{
    class FriendshipCommandRepository : DataAccess.EntityCommandRepository<FriendshipEntity>
    {
        protected override Command CreateInsertCommand(FriendshipEntity entity, IAuthenticatedUser user, string selector)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Friendship_Create")
                .AutoGenerateParameters(
                    qbeObject: entity
                )
                .OnBeforeCommandExecuted(cmd =>
            {
                var entities = Dependencies();

                var personEntity = (PersonEntity)entities.ElementAt(0).Entity;

                var friendEntity = (PersonEntity)entities.ElementAt(1).Entity;

                entity.Id = new FriendshipEntityId
                {
                    PersonId = personEntity.Id.Value,
                    FriendId = friendEntity.Id.Value
                };

                cmd.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("PersonId").Value(personEntity.Id),
                    p => p.Name("FriendId").Value(friendEntity.Id)
                );
            });
        }

    }
}
