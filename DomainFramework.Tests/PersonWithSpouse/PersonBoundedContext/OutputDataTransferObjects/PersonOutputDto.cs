using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace PersonWithSpouse.PersonBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? SpouseId { get; set; }

        public PersonOutputDto MarriedTo { get; set; }

    }
}