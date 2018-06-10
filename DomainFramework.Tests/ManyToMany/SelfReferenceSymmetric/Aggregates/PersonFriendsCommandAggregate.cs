using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class PersonFriendsCommandAggregate : CommandAggregate<PersonEntity>
    {
        private CollectionBinaryEntityLinkTransactedOperation<PersonEntity, PersonEntity, FriendshipEntity> _friendsLink { get; set; }

        public IEnumerable<PersonEntity> Friends => _friendsLink.LinkedEntities;

        public PersonFriendsCommandAggregate(RepositoryContext context, PersonEntity entity) : base(context, entity)
        {
            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<PersonEntity>(entity)
            );

            _friendsLink = new CollectionBinaryEntityLinkTransactedOperation<PersonEntity, PersonEntity, FriendshipEntity>(entity);

            TransactedSaveOperations.Enqueue(_friendsLink);
        }

        // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        public void AddFriend(PersonEntity friend, DateTime acceptedDateTime)
        {
            _friendsLink.AddLinkedEntity(friend);

            _friendsLink.AddBinaryEntity(new FriendshipEntity { AcceptedDateTime = acceptedDateTime });
        }
    }
}
