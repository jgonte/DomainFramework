using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class CreateCapitalCityInputDto : IInputDataTransferObject
    {
        public string Name { get; set; }

        public string CountryCode { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 150);

            CountryCode.ValidateMaxLength(result, nameof(CountryCode), 2);
        }

    }
}