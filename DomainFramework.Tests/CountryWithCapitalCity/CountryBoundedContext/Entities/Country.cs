using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class Country : Entity<string>
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}