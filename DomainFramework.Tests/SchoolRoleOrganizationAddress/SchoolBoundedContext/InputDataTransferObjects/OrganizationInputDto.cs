using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationInputDto : IInputDataTransferObject
    {
        public int OrganizationId { get; set; }

        public string Name { get; set; }

        public int? AddressId { get; set; }

        public OrganizationRoleInputDto OrganizationRole { get; set; }

        public PhoneInputDto Phone { get; set; }

        /// <summary>
        /// The address where the organization is located
        /// </summary>
        public AddressInputDto Address { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Name.ValidateRequired(result, nameof(Name));

            Name.ValidateMaxLength(result, nameof(Name), 50);

            OrganizationRole.Validate(result);

            Phone.Validate(result);

            Address?.Validate(result);
        }

    }
}