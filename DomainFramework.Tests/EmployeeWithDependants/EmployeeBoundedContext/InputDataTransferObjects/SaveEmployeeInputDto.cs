using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class SaveEmployeeInputDto : PersonInputDto
    {
        public int EmployeeId { get; set; }

        public DateTime HireDate { get; set; }

        public PhoneNumberInputDto CellPhone { get; set; }

        public List<PersonInputDto> Dependants { get; set; } = new List<PersonInputDto>();

        public override void Validate(ValidationResult result)
        {
            base.Validate(result);

            HireDate.ValidateRequired(result, nameof(HireDate));

            CellPhone.Validate(result);

            foreach (var person in Dependants)
            {
                person.Validate(result);
            }
        }

    }
}