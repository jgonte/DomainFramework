using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class SaveEmployeeInputDto : IInputDataTransferObject
    {
        public int? EmployeeId { get; set; }

        public DateTime HireDate { get; set; }

        public string Name { get; set; }

        public char Gender { get; set; }

        public PhoneNumberInputDto CellPhone { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            HireDate.ValidateNotEmpty(result, nameof(HireDate));

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Gender.ValidateNotEmpty(result, nameof(Gender));

            CellPhone?.Validate(result);
        }

    }
}