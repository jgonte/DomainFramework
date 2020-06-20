using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? SpouseId { get; set; }

        public int? BestFriendId { get; set; }

        public PersonOutputDto MarriedTo { get; set; }

        public PersonOutputDto BestFriendOf { get; set; }

    }
}