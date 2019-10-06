using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CountryWithCapitalCity.CountryBoundedContext
{
    public class UpdateCountryInputDto : IInputDataTransferObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public UpdateCapitalCityInputDto CapitalCity { get; set; }

        public void Validate(ValidationResult result)
        {
            Id.ValidateNotEmpty(result, nameof(Id));

            Id.ValidateMaxLength(result, nameof(Id), 2);

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 150);

            CapitalCity?.Validate(result);
        }

    }
}