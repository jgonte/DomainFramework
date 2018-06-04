using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class PersonFriendsCommandAggregate : CommandAggregate<PersonEntity>
    {
        public CommandCollectionBinaryEntityLink<PersonEntity, PersonEntity, FriendshipEntity> PersonFriendsLink { get; set; } = new CommandCollectionBinaryEntityLink<PersonEntity, PersonEntity, FriendshipEntity>();

        public IEnumerable<PersonEntity> Friends => PersonFriendsLink.LinkedEntities;

        public PersonFriendsCommandAggregate(RepositoryContext context, PersonEntity entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<ICommandCollectionEntityLink<PersonEntity>>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(PersonFriendsLink);
        }

        // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        public void AddFriend(PersonEntity friend, DateTime acceptedDateTime)
        {
            PersonFriendsLink.AddEntity(friend);

            PersonFriendsLink.AddJoinEntity(new FriendshipEntity { AcceptedDateTime = acceptedDateTime });
        }
    }
}
