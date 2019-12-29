using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class PersonInputDto : IInputDataTransferObject
    {
        public string Name { get; set; }

        public int? ProviderEmployeeId { get; set; }

        public PhoneNumberInputDto CellPhone { get; set; }

        public void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            CellPhone?.Validate(result);
        }

    }
}