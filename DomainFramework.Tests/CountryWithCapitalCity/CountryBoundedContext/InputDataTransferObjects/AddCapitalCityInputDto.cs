using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class AddCapitalCityInputDto : IInputDataTransferObject
    {
        public string Name { get; set; }

        public string CountryCode { get; set; }

        public void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 150);

            CountryCode.ValidateNotEmpty(result, nameof(CountryCode));

            CountryCode.ValidateMaxLength(result, nameof(CountryCode), 2);
        }

    }
}