using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class PersonFriendsQueryAggregate : QueryAggregate<int?, PersonEntity>
    {
        public IEnumerable<PersonEntity> Friends => FriendLinks.LinkedEntities;

        public PersonFriendsQueryAggregate(RepositoryContext context) : base(context)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<IQueryCollectionEntityLink>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(FriendLinks);
        }

        public QueryCollectionFriendEntityLink FriendLinks { get; set; } = new QueryCollectionFriendEntityLink();
    }
}