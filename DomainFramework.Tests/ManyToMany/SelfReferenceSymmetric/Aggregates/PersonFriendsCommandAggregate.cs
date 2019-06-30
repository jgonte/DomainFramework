using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class PersonFriendsCommandAggregate : CommandAggregate<PersonEntity>
    {
        public PersonFriendsCommandAggregate(RepositoryContext context, PersonWithFriendsDto personWithFriends) : base(context)
        {
            RootEntity = new PersonEntity
            {
                FirstName = personWithFriends.FirstName
            };

            Enqueue(
                new SaveEntityCommandOperation<PersonEntity>(RootEntity)
            );

            foreach (var friend in personWithFriends.Friends)
            {
                var friendEntity = new PersonEntity
                {
                    FirstName = friend.FirstName
                };

                var createFriend = new SaveEntityCommandOperation<PersonEntity>(friendEntity);

                Enqueue(createFriend);

                var binaryEntity = new FriendshipEntity
                {
                    AcceptedDateTime = friend.AcceptedDateTime
                };

                var addBinaryEntity = new InsertEntityCommandOperation<FriendshipEntity>(
                    binaryEntity,
                    new IEntity [] { RootEntity, friendEntity}
                );

                Enqueue(addBinaryEntity);
            }

           // _friendsLink = new CollectionBinaryEntityLinkTransactedOperation<PersonEntity, PersonEntity, FriendshipEntity>(entity);

        }
    }
}
