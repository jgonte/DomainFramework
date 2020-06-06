using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace PersonWithDisciples.PersonBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? LeaderId { get; set; }

        public IEnumerable<PersonOutputDto> Disciples { get; set; }

    }
}