using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class SaveEmployeeInputDto : IInputDataTransferObject
    {
        public int? EmployeeId { get; set; }

        public DateTime HireDate { get; set; }

        public string Name { get; set; }

        public int? MarriedToPersonId { get; set; }

        public PhoneNumberInputDto CellPhone { get; set; }

        public PersonInputDto Spouse { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            HireDate.ValidateNotEmpty(result, nameof(HireDate));

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            CellPhone?.Validate(result);

            Spouse?.Validate(result);
        }

    }
}