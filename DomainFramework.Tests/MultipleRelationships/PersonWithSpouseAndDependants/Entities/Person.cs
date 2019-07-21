using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.PersonWithSpouseAndDependants
{
    public class Person : Entity<int?>
    {
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

        public int? MarriedPersonId { get; set; }

        public int? ProviderPersonId { get; set; }

    }
}