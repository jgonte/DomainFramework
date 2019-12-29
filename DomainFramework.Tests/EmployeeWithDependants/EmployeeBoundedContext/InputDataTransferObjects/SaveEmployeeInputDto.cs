using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class SaveEmployeeInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public DateTime HireDate { get; set; }

        public string Name { get; set; }

        public int? ProviderEmployeeId { get; set; }

        public List<PersonInputDto> Dependants { get; set; } = new List<PersonInputDto>();

        public SavePhoneNumberInputDto CellPhone { get; set; }

        public void Validate(ValidationResult result)
        {
            HireDate.ValidateNotEmpty(result, nameof(HireDate));

            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            foreach (var person in Dependants)
            {
                person.Validate(result);
            }

            CellPhone?.Validate(result);
        }

    }
}