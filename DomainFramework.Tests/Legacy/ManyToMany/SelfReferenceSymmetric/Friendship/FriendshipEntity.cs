using DomainFramework.Core;
using System;

namespace DomainFramework.Tests
{
    class FriendshipEntityId
    {
        public int PersonId { get; set; }

        public int FriendId { get; set; }
    }

    /// <summary>
    /// The join entity between class and students
    /// </summary>
    class FriendshipEntity : Entity<FriendshipEntityId>
    {
        public DateTime AcceptedDateTime { get; set; }
    }
}
