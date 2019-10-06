using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public int AddressId { get; set; }

        public AddressInputDto Address { get; set; }

        public PhoneInputDto Phone { get; set; }

        public void Validate(ValidationResult result)
        {
            Name.ValidateNotEmpty(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            AddressId.ValidateNotZero(result, nameof(AddressId));

            Address.ValidateNotNull(result, nameof(Address));

            Address?.Validate(result);

            Phone.ValidateNotNull(result, nameof(Phone));

            Phone?.Validate(result);
        }

    }
}