using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class AddressInputDto : IInputDataTransferObject
    {
        public string Street { get; set; }

        public void Validate(ValidationResult result)
        {
            Street.ValidateNotEmpty(result, nameof(Street));

            Street.ValidateMaxLength(result, nameof(Street), 50);
        }

    }
}