using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class PhoneNumberInputDto : IInputDataTransferObject
    {
        public string AreaCode { get; set; }

        public string Exchange { get; set; }

        public string Number { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            AreaCode.ValidateRequired(result, nameof(AreaCode));

            AreaCode.ValidateMaxLength(result, nameof(AreaCode), 3);

            Exchange.ValidateRequired(result, nameof(Exchange));

            Exchange.ValidateMaxLength(result, nameof(Exchange), 3);

            Number.ValidateRequired(result, nameof(Number));

            Number.ValidateMaxLength(result, nameof(Number), 4);
        }

    }
}