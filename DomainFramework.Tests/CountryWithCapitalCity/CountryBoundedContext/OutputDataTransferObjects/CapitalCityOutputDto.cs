using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CapitalCityOutputDto : IOutputDataTransferObject
    {
        public int CapitalCityId { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }

    }
}