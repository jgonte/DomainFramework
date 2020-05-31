using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CreateCountryInputDto : IInputDataTransferObject
    {
        public string CountryCode { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public CreateCapitalCityInputDto CapitalCity { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            CountryCode.ValidateNotEmpty(result, nameof(CountryCode));

            CountryCode.ValidateMaxLength(result, nameof(CountryCode), 2);

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 150);

            CapitalCity?.Validate(result);
        }

    }
}