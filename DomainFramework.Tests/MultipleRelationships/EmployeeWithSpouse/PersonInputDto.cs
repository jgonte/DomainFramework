using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class PersonInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int? MarriedToPersonId { get; set; }

        public PhoneNumberInputDto CellPhone { get; set; }

        public void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            CellPhone?.Validate(result);
        }

    }
}
