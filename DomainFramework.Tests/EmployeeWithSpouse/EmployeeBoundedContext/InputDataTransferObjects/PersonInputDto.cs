using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class PersonInputDto : IInputDataTransferObject
    {
        public int PersonId { get; set; }

        public string Name { get; set; }

        public int? MarriedToPersonId { get; set; }

        public PhoneNumberInputDto CellPhone { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            CellPhone?.Validate(result);
        }

    }
}