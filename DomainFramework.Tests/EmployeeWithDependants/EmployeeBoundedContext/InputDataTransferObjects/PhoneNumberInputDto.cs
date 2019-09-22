using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class PhoneNumberInputDto : IInputDataTransferObject
    {
        public string AreaCode { get; set; }

        public string Exchange { get; set; }

        public string Number { get; set; }

        public void Validate(ValidationResult result)
        {
            AreaCode.ValidateNotEmpty(result, nameof(AreaCode));

            AreaCode.ValidateMaxLength(result, nameof(AreaCode), 3);

            Exchange.ValidateNotEmpty(result, nameof(Exchange));

            Exchange.ValidateMaxLength(result, nameof(Exchange), 3);

            Number.ValidateNotEmpty(result, nameof(Number));

            Number.ValidateMaxLength(result, nameof(Number), 4);
        }

    }
}