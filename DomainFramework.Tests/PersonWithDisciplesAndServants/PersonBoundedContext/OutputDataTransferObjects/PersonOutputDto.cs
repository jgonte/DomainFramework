using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace PersonWithDisciplesAndServants.PersonBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public int? LeaderId { get; set; }

        public int? MasterId { get; set; }

        public IEnumerable<PersonOutputDto> Disciples { get; set; }

        public IEnumerable<PersonOutputDto> Servants { get; set; }

    }
}