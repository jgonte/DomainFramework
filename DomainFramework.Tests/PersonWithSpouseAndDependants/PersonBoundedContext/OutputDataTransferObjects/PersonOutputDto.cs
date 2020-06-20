using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace PersonWithSpouseAndDependants.PersonBoundedContext
{
    public class PersonOutputDto : IOutputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public int? MarriedPersonId { get; set; }

        public int? ProviderPersonId { get; set; }

        public PersonOutputDto Spouse { get; set; }

        public IEnumerable<PersonOutputDto> Dependants { get; set; }

    }
}