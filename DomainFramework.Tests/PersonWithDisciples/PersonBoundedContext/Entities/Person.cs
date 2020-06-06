using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace PersonWithDisciples.PersonBoundedContext
{
    public class Person : Entity<int?>
    {
        public string Name { get; set; }

        public char Gender { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? LeaderId { get; set; }

    }
}