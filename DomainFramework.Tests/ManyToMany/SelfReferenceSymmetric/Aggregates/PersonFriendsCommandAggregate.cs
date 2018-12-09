using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities;

namespace DomainFramework.Tests
{
    class PersonFriendsCommandAggregate : CommandAggregate<PersonEntity>
    {
        private CollectionBinaryEntityLinkTransactedOperation<PersonEntity, PersonEntity, FriendshipEntity> _friendsLink { get; set; }

        public IEnumerable<PersonEntity> Friends => _friendsLink.LinkedEntities;

        public PersonFriendsCommandAggregate(RepositoryContext context, PersonEntity entity) : base(context, entity)
        {
            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<PersonEntity>(entity, CommandOperationTypes.Save)
            );

            _friendsLink = new CollectionBinaryEntityLinkTransactedOperation<PersonEntity, PersonEntity, FriendshipEntity>(entity);

            TransactedOperations.Enqueue(_friendsLink);
        }

        // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        public void AddFriend(PersonEntity friend, DateTime acceptedDateTime)
        {
            _friendsLink.AddLinkedEntity(friend);

            _friendsLink.AddBinaryEntity(new FriendshipEntity { AcceptedDateTime = acceptedDateTime });
        }
    }
}
