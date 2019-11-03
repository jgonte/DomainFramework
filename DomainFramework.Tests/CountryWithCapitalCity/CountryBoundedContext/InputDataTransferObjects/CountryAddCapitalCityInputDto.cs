using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CountryAddCapitalCityInputDto : IInputDataTransferObject
    {
        public string Id { get; set; }

        public AddCapitalCityInputDto CapitalCity { get; set; }

        public void Validate(ValidationResult result)
        {
            Id.ValidateNotEmpty(result, nameof(Id));

            Id.ValidateMaxLength(result, nameof(Id), 2);

            CapitalCity?.Validate(result);
        }

    }
}