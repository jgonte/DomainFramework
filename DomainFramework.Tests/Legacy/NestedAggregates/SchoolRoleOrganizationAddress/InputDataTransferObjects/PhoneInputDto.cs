using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class PhoneInputDto : IInputDataTransferObject
    {
        public string Number { get; set; }

        public void Validate(ValidationResult result)
        {
            Number.ValidateNotEmpty(result, nameof(Number));

            Number.ValidateMaxLength(result, nameof(Number), 10);
        }

    }
}