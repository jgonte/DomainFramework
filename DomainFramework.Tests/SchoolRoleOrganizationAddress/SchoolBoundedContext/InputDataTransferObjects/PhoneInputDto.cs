using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class PhoneInputDto : IInputDataTransferObject
    {
        public string Number { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Number.ValidateNotEmpty(result, nameof(Number));

            Number.ValidateMaxLength(result, nameof(Number), 10);
        }

    }
}