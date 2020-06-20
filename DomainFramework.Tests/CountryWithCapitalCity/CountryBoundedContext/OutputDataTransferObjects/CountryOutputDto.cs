using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryOutputDto : IOutputDataTransferObject
    {
        public string CountryCode { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public CapitalCityOutputDto CapitalCity { get; set; }

    }
}