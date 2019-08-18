using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class SaveEmployeeInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public DateTime HireDate { get; set; }

        public string Name { get; set; }

        public int? MarriedToPersonId { get; set; }

        public PersonInputDto Spouse { get; set; }

        public SavePhoneNumberInputDto CellPhone { get; set; }

        public void Validate(ValidationResult result)
        {
            HireDate.ValidateNotEmpty(result, nameof(HireDate));

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            Spouse?.Validate(result);

            CellPhone?.Validate(result);
        }

    }
}
