using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class AddressInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public string Street { get; set; }

        public void Validate(ValidationResult result)
        {
            Street.ValidateNotEmpty(result, nameof(Street));

            Street.ValidateMaxLength(result, nameof(Street), 50);
        }

    }
}