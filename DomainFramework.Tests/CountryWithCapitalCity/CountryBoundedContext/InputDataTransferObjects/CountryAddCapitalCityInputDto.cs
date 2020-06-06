using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryAddCapitalCityInputDto : IInputDataTransferObject
    {
        public string CountryCode { get; set; }

        public CreateCapitalCityInputDto CapitalCity { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            CountryCode.ValidateNotEmpty(result, nameof(CountryCode));

            CountryCode.ValidateMaxLength(result, nameof(CountryCode), 2);

            CapitalCity?.Validate(result);
        }

    }
}